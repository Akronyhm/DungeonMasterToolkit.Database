using DungeonMasterToolkit.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Note_AspNetUser : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Note_AspNetUser>()
            .ToTable("X_Note_AspNetUser");

        modelBuilder.Entity<X_Note_AspNetUser>()
            .HasKey(x => new { x.X_Note_Id, x.AspNetUser_Id });

        modelBuilder.Entity<X_Note_AspNetUser>()
            .Property(x => x.AspNetUser_Id)
            .HasMaxLength(450)
            .IsRequired();

        modelBuilder.Entity<X_Note_AspNetUser>()
            .Property(x => x.X_Note_Id)
            .IsRequired();

        modelBuilder.Entity<X_Note_AspNetUser>()
            .Property(x => x.CanEdit)
            .HasDefaultValue(false)
            .IsRequired();

        modelBuilder.Entity<X_Note_AspNetUser>()
            .HasOne(x => x.X_Note)
            .WithMany()
            .HasForeignKey(x => x.X_Note_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<X_Note_AspNetUser>()
            .HasOne(x => x.ApplicationUser)
            .WithMany()
            .HasForeignKey(x => x.AspNetUser_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public long X_Note_Id { get; set; }
    public string AspNetUser_Id { get; set; } = "";
    public bool CanEdit { get; set; } = false;

    public virtual X_Note X_Note { get; set; } = default!;
    public virtual ApplicationUser ApplicationUser { get; set; } = default!;
}
