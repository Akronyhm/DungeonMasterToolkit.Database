using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Note_X_Character : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Note_X_Character>()
            .ToTable("X_Note_X_Character");

        modelBuilder.Entity<X_Note_X_Character>()
            .HasKey(x => new { x.X_Note_Id, x.X_Character_Id });

        modelBuilder.Entity<X_Note_X_Character>()
            .Property(x => x.X_Note_Id).IsRequired();

        modelBuilder.Entity<X_Note_X_Character>()
            .Property(x => x.X_Character_Id).IsRequired();

        modelBuilder.Entity<X_Note_X_Character>()
            .HasOne(x => x.X_Note)
            .WithMany()
            .HasForeignKey(x => x.X_Note_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Note_X_Character>()
            .HasOne(x => x.X_Character)
            .WithMany()
            .HasForeignKey(x => x.X_Character_Id)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public long X_Note_Id { get; set; }
    public long X_Character_Id { get; set; }
    public virtual X_Note X_Note { get; set; } = default!;
    public virtual X_Character X_Character { get; set; } = default!;
}
