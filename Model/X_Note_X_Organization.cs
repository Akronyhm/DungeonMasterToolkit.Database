using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Note_X_Organization : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Note_X_Organization>()
            .ToTable("X_Note_X_Organization");

        modelBuilder.Entity<X_Note_X_Organization>()
            .HasKey(x => new { x.X_Note_Id, x.X_Organization_Id });

        modelBuilder.Entity<X_Note_X_Organization>()
            .Property(x => x.X_Note_Id).IsRequired();

        modelBuilder.Entity<X_Note_X_Organization>()
            .Property(x => x.X_Organization_Id).IsRequired();

        modelBuilder.Entity<X_Note_X_Organization>()
            .HasOne(x => x.X_Note)
            .WithMany()
            .HasForeignKey(x => x.X_Note_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Note_X_Organization>()
            .HasOne(x => x.X_Organization)
            .WithMany()
            .HasForeignKey(x => x.X_Organization_Id)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public long X_Note_Id { get; set; }
    public long X_Organization_Id { get; set; }
    public virtual X_Note X_Note { get; set; } = default!;
    public virtual X_Organization X_Organization { get; set; } = default!;
}
