using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Organization_X_Location : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Organization_X_Location>()
            .ToTable("X_Organization_X_Location");

        modelBuilder.Entity<X_Organization_X_Location>()
            .HasKey(x => new { x.X_Organization_Id, x.X_Location_Id });

        modelBuilder.Entity<X_Organization_X_Location>()
            .Property(x => x.X_Organization_Id).IsRequired();

        modelBuilder.Entity<X_Organization_X_Location>()
            .Property(x => x.X_Location_Id).IsRequired();

        modelBuilder.Entity<X_Organization_X_Location>()
            .HasOne(x => x.X_Organization)
            .WithMany()
            .HasForeignKey(x => x.X_Organization_Id)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<X_Organization_X_Location>()
            .HasOne(x => x.X_Location)
            .WithMany()
            .HasForeignKey(x => x.X_Location_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Organization_X_Location>()
            .Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
    }

    public long X_Organization_Id { get; set; }
    public long X_Location_Id { get; set; }
    public DateTimeOffset Created { get; set; }

    public virtual X_Organization X_Organization { get; set; } = default!;
    public virtual X_Location X_Location { get; set; } = default!;
}
