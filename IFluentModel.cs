using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database;

public interface IFluentModel
{
    void OnModelCreating(ModelBuilder modelBuilder);
}
