using DungeonMasterToolkit.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterToolkit.Database;

public class Feature : IComparable<Feature>, IComparable<Core_Version>
{

    /// <summary>
    /// Determines in what order we can expect this feature to run. Multiple features sharing the same phase will still order themselves due to regular comparison, but all features of phase 1, will run before phase 2, which will in turn run before phase 3 etc...
    /// </summary>
    public int Phase { get; set; } = 1;
    /// <summary>
    /// Feature name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Date-string formatted as yyyy-mm-dd
    /// </summary>
    public string Date { get; set; }
    /// <summary>
    /// Version number (2 digits)
    /// </summary>
    public string Version { get; set; }
    public Assembly Assembly { get; set; } = null;
    private static int ParseVer(string? s) => int.TryParse(s, out var i) ? i : 0;
    
    public int CompareTo(Feature? other)
    {
        if (other is null) return 1;

        var p = Phase.CompareTo(other.Phase);
        if (p != 0) return p;

        var d = string.Compare(Date, other.Date, StringComparison.Ordinal);
        if (d != 0) return d;

        var v = ParseVer(Version).CompareTo(ParseVer(other.Version));
        if (v != 0) return v;

        return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }
    public int CompareTo(Core_Version? other)
    {
        if (other is null) return 1;

        // Core_Version doesn't have Phase: if you need downgrade comparisons across phases,
        // consider adding Phase to Core_Version too.
        var d = string.Compare(Date, other.Date, StringComparison.Ordinal);
        if (d != 0) return d;

        var v = ParseVer(Version).CompareTo(ParseVer(other.Version));
        if (v != 0) return v;

        return string.Compare(Name, other.DatabaseFeature, StringComparison.OrdinalIgnoreCase);
    }
    public override string ToString()
    {
        return $"{Phase}.{Name}.{Date}.{Version.PadLeft(2, '0')}";
    }
    public static bool operator <(Feature first, Feature second)
    {
        return first.CompareTo(second) < 0;
    }
    public static bool operator >(Feature first, Feature second)
    {
        return first.CompareTo(second) > 0;
    }
    public static bool operator <(Feature first, Core_Version second)
    {
        return first.CompareTo(second) < 0;
    }
    public static bool operator >(Feature first, Core_Version second)
    {
        return first.CompareTo(second) > 0;
    }
}
