using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonMasterToolkit.Database.Model;

public partial class Core_Version
{
    [Key]
    public long Id { get; set; }
    public string DatabaseFeature { get; set; }
    public string Date { get; set; }
    public string Version { get; set; }
    public string UninstallScript { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset Created { get; set; }
    public override string ToString()
    {
        return $"{DatabaseFeature}.{Date}.{Version}";
    }
}
