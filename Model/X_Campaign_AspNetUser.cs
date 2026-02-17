using DungeonMasterToolkit.Database.Utils;
using DungeonMasterToolkit.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;
public class X_Campaign_AspNetUser : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        var created = modelBuilder.Entity<X_Campaign_AspNetUser>().ToTable("X_Campaign_AspNetUser");

        created.HasKey(x => new { x.AspNetUsers_Id, x.X_Campaign_Id});

        created.Property(x => x.AspNetUsers_Id)
            .HasMaxLength(450)
            .IsRequired();
        created.Property(x => x.X_Campaign_Id)
            .IsRequired();
        created.Property(x => x.AccessLevel)
            .HasDefaultValue(CampaignAccessLevelEnum.Member)
            .HasSentinel(CampaignAccessLevelEnum.Unspecified)
            .IsRequired();
        created.Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        created.HasOne(x => x.X_Campaign)
            .WithMany()
            .HasForeignKey(x => x.X_Campaign_Id)
            .OnDelete(DeleteBehavior.Cascade);

        created.HasOne(x => x.ApplicationUser)
            .WithMany()
            .HasForeignKey(x => x.AspNetUsers_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public long X_Campaign_Id { get; set; }
    public string AspNetUsers_Id { get; set; } = "";
    public CampaignAccessLevelEnum AccessLevel { get; set; } = CampaignAccessLevelEnum.Unspecified;
    public DateTimeOffset Created { get; set; }
    public virtual X_Campaign X_Campaign { get; set; } = default!;
    public virtual ApplicationUser ApplicationUser { get; set; } = default!;
}
