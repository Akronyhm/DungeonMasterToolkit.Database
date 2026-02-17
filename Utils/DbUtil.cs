using DungeonMasterToolkit.Database.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DungeonMasterToolkit.Database;
public static class DbUtil
{
    #region SQL Commands
    public static string EXISTSCMD =
@"
SELECT TOP 1 1 
as 
    [Exists] 
from 
    INFORMATION_SCHEMA.TABLES 
WHERE 
    TABLE_NAME = @TableName
";
    #endregion
    public static bool Exists(string connectionString, string tableName)
    {
        return ExistsAsync(connectionString, tableName).GetAwaiter().GetResult();
    }
    public static async Task<bool> ExistsAsync(string connectionString, string tableName)
    {
        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(EXISTSCMD, conn))
        {
            cmd.Parameters.AddWithValue("TableName", tableName);
            await conn.OpenAsync();
            var res = await cmd.ExecuteScalarAsync();
            return res != null && false == res is DBNull && (int)res == 1;
        }
    }
    public static void WaitForDatabase(string connectionString, int waitSeconds = 20)
    {
        WaitForDatabaseAsync(connectionString, waitSeconds).GetAwaiter().GetResult();
    }
    public static async Task WaitForDatabaseAsync(string connectionString, int waitSeconds = 20)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        builder.ConnectTimeout = 5;
        builder.ConnectRetryCount = 0;
        var start = DateTime.Now;
        while (true)
        {
            try
            {
                using (var conn = new SqlConnection(builder.ConnectionString))
                {
                    await conn.OpenAsync();
                    Common.Logging.Log.Info("Connected to database");
                    return;
                }
            }
            catch (Exception e)
            {
                Common.Logging.Log.Warn($"Failed to connect to database", e);
            }

            int elapsed = (int)(DateTime.Now - start).TotalSeconds;
            if (elapsed >= waitSeconds)
            {
                Common.Logging.Log.Error("Connection attempts to database have timed out");
                throw new Exception($"Connection attempts to database timed out ({waitSeconds} seconds)");
            }

            Common.Logging.Log.Warn($"Waiting for connection with database... {elapsed}s");
            System.Threading.Thread.Sleep(1000);
        }
    }
    public static void Update(
    string connectionString,
    List<Feature> features,
    bool allowDowngrade = false,
    int? commandTimeoutInSeconds = null)
    {
        // 1) Targets: one "latest requested" per feature name (case-insensitive)
        var targets = features
            .Where(f => !(allowDowngrade && f.Date == "0000-00-00")) // keep legacy escape hatch
            .GroupBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Max(), StringComparer.OrdinalIgnoreCase);

        // 2) Collect scripts from the assemblies requested
        var additionalAssemblies = features
            .Where(o => o.Assembly != null)
            .Select(o => o.Assembly!)
            .Distinct()
            .ToList();

        var updateScripts = EmbeddedUpdateScript.GetUpdateScripts(additionalAssemblies);
        updateScripts.Sort(); // relies on your Feature.CompareTo via script sorting

        foreach (var t in targets)
        {
            foreach (var s in updateScripts)
            {
                if (s.Feature.Name.Equals(t.Value.Name))
                {
                    s.Feature.Phase = t.Value.Phase;
                }
            }
        }

        Common.Logging.Log.Info("Updating database");

        // 3) Validate: every requested target exists as an embedded script
        foreach (var target in targets.Values)
        {
            var exists = updateScripts.Any(x =>
                x.Feature.Name.Equals(target.Name, StringComparison.OrdinalIgnoreCase) &&
                x.Feature.Date == target.Date &&
                x.Feature.Version == target.Version);

            if (!exists)
                throw new Exception($"Requested feature {target} does not exist as an embedded file");
        }

        // 4) Load installed features (Core_Version)
        var installed = GetInstalledFeatures(connectionString);

        // Build fast lookups
        static string Key(string name, string date, string version) =>
            $"{name.ToUpperInvariant()}|{date}|{version}";

        var installedSet = new HashSet<string>(
            installed.Select(v => Key(v.DatabaseFeature, v.Date, v.Version)));

        // For downgrade checks/uninstalls: "latest installed per name"
        int ParseVer(string? s) => int.TryParse(s, out var i) ? i : 0;

        Core_Version? LatestInstalledFor(string name) =>
            installed
                .Where(x => x.DatabaseFeature.Equals(name, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => ParseVer(x.Version))
                .FirstOrDefault();

        // 5) If downgrades not allowed: DB must not be ahead of targets
        if (!allowDowngrade)
        {
            foreach (var (name, target) in targets)
            {
                var cur = LatestInstalledFor(name);
                if (cur != null && target.CompareTo(cur) < 0) // Feature vs Core_Version CompareTo
                {
                    throw new DowngradeException(
                        $"Database version is greater than requested and downgrades are not allowed. DB[{cur}] Requested[{target}]");
                }
            }
        }

        string lastRunBatch = "";

        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    // 6) Upgrade pass: apply scripts up to target
                    foreach (var script in updateScripts)
                    {
                        if (!targets.TryGetValue(script.Feature.Name, out var target))
                            continue; // not requested

                        // Gate by target: ignore scripts newer than target
                        if (script.Feature.CompareTo(target) > 0)
                            continue;

                        var k = Key(script.Feature.Name, script.Feature.Date, script.Feature.Version);
                        if (installedSet.Contains(k))
                        {
                            Common.Logging.Log.Info($"Feature [{script.Feature}] already installed");
                            continue;
                        }

                        Common.Logging.Log.Info($"Installing script [{script}]");

                        foreach (var batch in Regex.Split(script.ReadInstallScript(), @"\s+GO\s+"))
                        {
                            if (string.IsNullOrWhiteSpace(batch))
                                continue;

                            Common.Logging.Log.Info($"Applying batch{Environment.NewLine}{batch}");
                            lastRunBatch = batch;

                            using (var cmd = new SqlCommand(batch, conn, trans))
                            {
                                if (commandTimeoutInSeconds.HasValue)
                                    cmd.CommandTimeout = commandTimeoutInSeconds.Value;

                                cmd.ExecuteNonQuery();
                            }
                        }

                        const string insert =
    @"
INSERT INTO [dbo].[Core_Version] ([Date],[Version],[DatabaseFeature],[UninstallScript])
VALUES (@Date,@Version,@Feature,@UninstallScript);
";

                        using (var cmd = new SqlCommand(insert, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@Date", script.Feature.Date);
                            cmd.Parameters.AddWithValue("@Version", script.Feature.Version);
                            cmd.Parameters.AddWithValue("@Feature", script.Feature.Name);
                            cmd.Parameters.Add("@UninstallScript", System.Data.SqlDbType.NVarChar, -1)
                               .Value = script.ReadUninstallScript();
                            cmd.ExecuteNonQuery();
                        }

                        installedSet.Add(k);
                        installed.Add(new Core_Version
                        {
                            DatabaseFeature = script.Feature.Name,
                            Date = script.Feature.Date,
                            Version = script.Feature.Version,
                            UninstallScript = script.ReadUninstallScript()
                        });
                    }

                    // 7) Downgrade pass (optional): uninstall anything newer than target
                    if (allowDowngrade)
                    {
                        var toRemove = installed
                            .Where(x => targets.TryGetValue(x.DatabaseFeature, out var target) && target.CompareTo(x) < 0)
                            .OrderByDescending(x => x.Date)
                            .ThenByDescending(x => ParseVer(x.Version))
                            .ToList();

                        if (toRemove.Count == 0)
                        {
                            Common.Logging.Log.Info("Database is correct version, nothing to uninstall");
                        }

                        foreach (var v in toRemove)
                        {
                            Common.Logging.Log.Info($"Uninstalling feature [{v.DatabaseFeature}.{v.Date}.{v.Version}]");

                            var hasDeletedCoreVersionTable = false;

                            foreach (var batch in Regex.Split(v.UninstallScript ?? "", @"\s+GO\s+"))
                            {
                                if (string.IsNullOrWhiteSpace(batch))
                                    continue;

                                hasDeletedCoreVersionTable = batch.Contains("DROP TABLE [dbo].[Core_Version]");
                                Common.Logging.Log.Info($"Applying batch{Environment.NewLine}{batch}");

                                using (var cmd = new SqlCommand(batch, conn, trans))
                                {
                                    if (commandTimeoutInSeconds.HasValue)
                                        cmd.CommandTimeout = commandTimeoutInSeconds.Value;

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            if (!hasDeletedCoreVersionTable)
                            {
                                const string delete =
    @"
DELETE FROM [dbo].[Core_Version]
WHERE [DatabaseFeature] = @Feature
  AND [Date] = @Date
  AND [Version] = @Version;
";
                                using (var cmd = new SqlCommand(delete, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@Feature", v.DatabaseFeature);
                                    cmd.Parameters.AddWithValue("@Date", v.Date);
                                    cmd.Parameters.AddWithValue("@Version", v.Version);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    trans.Commit();
                }
            }
        }
        catch (Exception e)
        {
            Common.Logging.Log.Error($"Failed to apply batch {Environment.NewLine}{lastRunBatch}", e);
            throw;
        }

        Common.Logging.Log.Info("Database update complete");
    }


    public static List<Core_Version> GetInstalledFeatures(string connStr)
    {
        var exists = Exists(connStr, "Core_Version");
        if (exists)
        {
            var features = new List<Core_Version>();
            features.AddRange([.. FContext.GetContext(connStr).Core_Version.AsNoTracking()]);
            return features;
        }

        return [];
    }
}

public class DowngradeException : Exception
{
    public DowngradeException(string message) : base(message) { }
}

public class EmbeddedUpdateScript : IComparable<EmbeddedUpdateScript>
{
    private static System.Text.RegularExpressions.Regex scriptRegex = new(@"(\w*)\.(\d{4}-\d{2}-\d{2}).(\d{2}).Install.sql$");
    public static List<EmbeddedUpdateScript> GetUpdateScripts(List<Assembly> additionalUpdateScriptAssemblies = null)
    {
        var assemblies = new List<Assembly> { Assembly.GetCallingAssembly() };
        if (assemblies != null)
        {
            assemblies.AddRange(additionalUpdateScriptAssemblies);
        }

        assemblies = assemblies.Distinct().ToList();

        var res = new List<EmbeddedUpdateScript>();
        foreach (var ass in assemblies)
        {
            var files = ass.GetManifestResourceNames();
            foreach (var file in files)
            {
                var match = scriptRegex.Match(file);
                if (match.Success)
                {
                    var script = new EmbeddedUpdateScript(ass, file, file.Replace("Install", "Uninstall"), new() { Name = match.Groups[1].Value, Date = match.Groups[2].Value, Version = match.Groups[3].Value });
                    res.Add(script);
                    if (files.FirstOrDefault(x => x.EndsWith(script.FullNameInstallScript)) == null)
                    {
                        throw new Exception($"Could not find script [{script.FullNameInstallScript}]");
                    }
                    if (files.FirstOrDefault(x => x.EndsWith(script.FullNameUninstallScript)) == null)
                    {
                        throw new Exception($"Could not find script [{script.FullNameUninstallScript}]");
                    }
                }
            }
        }
        return res;
    }

    public string FullNameInstallScript { get; set; }
    public string FullNameUninstallScript { get; set; }
    public Feature Feature { get; private set; }
    public Assembly Assembly { get; private set; }

    public EmbeddedUpdateScript(Assembly assembly, string installScript, string uninstallScript, Feature feature)
    {
        this.Assembly = assembly;
        this.FullNameInstallScript = installScript;
        this.FullNameUninstallScript = uninstallScript;
        this.Feature = feature;
    }
    public override string ToString()
    {
        return $"InstallScript[{FullNameInstallScript}] UninstallScript[{FullNameUninstallScript}]";
    }
    public string ReadInstallScript()
    {
        return ReadInstallScriptAsync().GetAwaiter().GetResult();
    }
    public async Task<string> ReadInstallScriptAsync()
    {
        using (var stream = Assembly.GetManifestResourceStream(FullNameInstallScript))
        using (var reader = new StreamReader(stream))
        {
            return await reader.ReadToEndAsync();
        }
    }
    public string ReadUninstallScript()
    {
        return ReadUninstallScriptAsync().GetAwaiter().GetResult();
    }
    public async Task<string> ReadUninstallScriptAsync()
    {
        using (var stream = Assembly.GetManifestResourceStream(FullNameUninstallScript))
        using (var reader = new StreamReader(stream))
        {
            return await reader.ReadToEndAsync();
        }
    }
    public int CompareTo(EmbeddedUpdateScript? other)
    {
        return Feature.CompareTo(other.Feature);
    }
}