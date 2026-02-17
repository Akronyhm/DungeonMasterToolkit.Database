using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Organization_X_Character : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Organization_X_Character>()
            .ToTable("X_Organization_X_Character");

        modelBuilder.Entity<X_Organization_X_Character>()
            .HasKey(x => new { x.X_Organization_Id, x.X_Character_Id });

        modelBuilder.Entity<X_Organization_X_Character>()
            .Property(x => x.X_Organization_Id).IsRequired();

        modelBuilder.Entity<X_Organization_X_Character>()
            .Property(x => x.X_Character_Id).IsRequired();

        modelBuilder.Entity<X_Organization_X_Character>()
            .HasOne(x => x.X_Organization)
            .WithMany()
            .HasForeignKey(x => x.X_Organization_Id)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<X_Organization_X_Character>()
            .HasOne(x => x.X_Character)
            .WithMany()
            .HasForeignKey(x => x.X_Character_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Organization_X_Character>()
            .Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
    }

    public long X_Organization_Id { get; set; }
    public long X_Character_Id { get; set; }
    public DateTimeOffset Created { get; set; }
    public virtual X_Organization X_Organization { get; set; } = default!;
    public virtual X_Character X_Character { get; set; } = default!;
}
