using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database.Model;

public class X_Note : IFluentModel
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<X_Note>()
            .ToTable("X_Note");

        modelBuilder.Entity<X_Note>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<X_Note>()
            .Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<X_Note>()
            .Property(x=>x.NoteId)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Note>()
            .Property(x => x.Created)
            .HasDefaultValueSql("sysdatetimeoffset()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

        modelBuilder.Entity<X_Note>()
            .HasOne(x => x.X_Campaign)
            .WithMany()
            .HasForeignKey(x => x.X_Campaign_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public long Id { get; set; }
    public Guid NoteId { get; set; }
    public string? Text { get; set; }
    public long X_Campaign_Id { get; set; }
    public DateTimeOffset Created { get; set; }

    public virtual X_Campaign X_Campaign { get; set; } = default!;
}
