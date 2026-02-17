using DungeonMasterToolkit.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_PersistentUserState : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_PersistentUserState>()
            .ToTable("X_PersistentUserState");

        modelBuilder.Entity<X_PersistentUserState>()
            .HasKey(x => x.UserId);

        modelBuilder.Entity<X_PersistentUserState>()
            .HasOne(x => x.ApplicationUser)
            .WithOne()
            .HasForeignKey<X_PersistentUserState>(x => x.UserId);

        modelBuilder.Entity<X_PersistentUserState>()
            .Property(x => x.ActiveCampaignId)
            .IsRequired(false);
    }

    public string UserId { get; set; } = "";

    public long? ActiveCampaignId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; } = default!;
}
