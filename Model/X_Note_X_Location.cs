using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Note_X_Location : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Note_X_Location>()
            .ToTable("X_Note_X_Location");

        modelBuilder.Entity<X_Note_X_Location>()
            .HasKey(x => new { x.X_Note_Id, x.X_Location_Id });

        modelBuilder.Entity<X_Note_X_Location>()
            .Property(x => x.X_Note_Id).IsRequired();

        modelBuilder.Entity<X_Note_X_Location>()
            .Property(x => x.X_Location_Id).IsRequired();

        modelBuilder.Entity<X_Note_X_Location>()
            .HasOne(x => x.X_Note)
            .WithMany()
            .HasForeignKey(x => x.X_Note_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Note_X_Location>()
            .HasOne(x => x.X_Location)
            .WithMany()
            .HasForeignKey(x => x.X_Location_Id)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public long X_Note_Id { get; set; }
    public long X_Location_Id { get; set; }

    public virtual X_Note X_Note { get; set; } = default!;
    public virtual X_Location X_Location { get; set; } = default!;
}
