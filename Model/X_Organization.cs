using DungeonMasterToolkit.Database.Utils;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Organization : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Organization>()
            .ToTable("X_Organization");

        modelBuilder.Entity<X_Organization>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<X_Organization>()
            .Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<X_Organization>()
            .Property(x => x.Disposition)
            .HasDefaultValue(DispositionEnum.Neutral)
            .HasSentinel(DispositionEnum.Undefined)
            .IsRequired();

        modelBuilder.Entity<X_Organization>()
            .Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Organization>()
            .HasOne(x => x.X_Campaign)
            .WithMany()
            .HasForeignKey(x => x.X_Campaign_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Organization>()
            .Property(x => x.OrganizationId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Organization>()
            .HasIndex(x => x.OrganizationId)
            .IsUnique();

    }

    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public DispositionEnum Disposition { get; set; }
    public long X_Campaign_Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTimeOffset Created { get; set; }
    public virtual X_Campaign X_Campaign { get; set; } = default!;
}
