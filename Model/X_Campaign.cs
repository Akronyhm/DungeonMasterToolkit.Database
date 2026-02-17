using DungeonMasterToolkit.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DungeonMasterToolkit.Database.Model;
public class X_Campaign : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Campaign>()
            .ToTable("X_Campaign");

        modelBuilder.Entity<X_Campaign>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<X_Campaign>()
            .HasIndex(x => x.CampaignId).IsUnique();

        modelBuilder.Entity<X_Campaign>()
            .Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<X_Campaign>()
            .Property(x => x.CampaignId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<X_Campaign>()
            .Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);


        modelBuilder.Entity<X_Campaign>()
            .Property(x => x.Name).HasMaxLength(128).IsRequired();
        
        modelBuilder.Entity<X_Campaign>()
            .Property(x => x.Notes).HasMaxLength(500);
    }

    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string? Notes { get; set; }

    /// <summary>
    /// GUID that identifies this particular Campaign
    /// </summary>
    public Guid CampaignId { get; set; }

    /// <summary>
    /// Date when this campaign was created
    /// </summary>
    public DateTimeOffset Created { get; set; }

}
