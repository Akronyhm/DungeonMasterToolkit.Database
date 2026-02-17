namespace DungeonMasterToolkit.Database;

public class FDatabaseOptions
{
    public string ConnectionString { get; set; }

    public int? DatabaseUpdateCommandTimeoutInSeconds { get; set; }

    public bool AllowDowngrade { get; set; }

    public List<Feature> DatabaseVersion { get; set; }
}
