using DungeonMasterToolkit.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterToolkit.Database
{
    public partial class FContext
    {
        public virtual DbSet<Core_Version> Core_Version { get; set; }
        public virtual DbSet<X_Campaign> X_Campaign { get; set; }
        public virtual DbSet<X_Campaign_AspNetUser> X_Campaign_AspNetUser { get; set; }
        public virtual DbSet<X_Organization> X_Organization { get; set; }
        public virtual DbSet<X_Character> X_Character { get; set; }
        public virtual DbSet<X_Location> X_Location { get; set; }
        public virtual DbSet<X_Note> X_Note { get; set; }
        public virtual DbSet<X_Note_X_Organization> X_Note_X_Organization { get; set; }
        public virtual DbSet<X_Note_X_Character> X_Note_X_Character { get; set; }
        public virtual DbSet<X_Note_X_Location> X_Note_X_Location { get; set; }
        public virtual DbSet<X_Note_AspNetUser> X_Note_AspNetUser { get; set; }

        public virtual DbSet<X_Organization_X_Location> X_Organization_X_Location { get; set; }
        public virtual DbSet<X_Organization_X_Character> X_Organization_X_Character { get; set; }
        public virtual DbSet<X_PersistentUserState> X_PersistentUserState { get; set; }

    }
}
