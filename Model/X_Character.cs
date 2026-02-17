using DungeonMasterToolkit.Database.Utils;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Character : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Character>()
            .ToTable("X_Character");
        
        modelBuilder.Entity<X_Character>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<X_Character>()
            .Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<X_Character>()
            .Property(x => x.IsAlive).HasDefaultValue(true);

        modelBuilder.Entity<X_Character>()
            .Property(x => x.Disposition)
            .HasDefaultValue(DispositionEnum.Neutral)
            .HasSentinel(DispositionEnum.Undefined)
            .IsRequired();

        modelBuilder.Entity<X_Character>()
            .Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Character>()
            .HasOne(x => x.X_Campaign)
            .WithMany()
            .HasForeignKey(x => x.X_Campaign_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Character>()
            .Property(x => x.CharacterId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Character>()
            .HasIndex(x => x.CharacterId)
            .IsUnique();
    }

    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public bool IsAlive { get; set; } = true;
    public DispositionEnum Disposition { get; set; }
    public long X_Campaign_Id { get; set; }
    public Guid CharacterId { get; set; }
    public DateTimeOffset Created { get; set; }

    public virtual X_Campaign X_Campaign { get; set; } = default!;
}
