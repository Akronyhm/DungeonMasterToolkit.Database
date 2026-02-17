using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Location : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Location>()
            .ToTable("X_Location");

        modelBuilder.Entity<X_Location>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<X_Location>()
            .Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<X_Location>()
            .Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Location>()
            .HasOne(x => x.X_Campaign)
            .WithMany()
            .HasForeignKey(x => x.X_Campaign_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Location>()
            .Property(x => x.LocationId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Location>()
            .HasIndex(x => x.LocationId)
            .IsUnique();
    }

    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public long X_Campaign_Id { get; set; }
    public Guid LocationId { get; set; }
    public DateTimeOffset Created { get; set; }
    public virtual X_Campaign X_Campaign { get; set; } = default!;
}
