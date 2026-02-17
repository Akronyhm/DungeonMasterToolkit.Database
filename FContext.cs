using DungeonMasterToolkit.Common.Utilities;
using DungeonMasterToolkit.Web.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DungeonMasterToolkit.Database
{
    public partial class FContext : IdentityDbContext<ApplicationUser>
    {
        public static string ConnectionString { get; set; }
        public static bool UseHistTables { get; set; } = false;
        public static Func<DateTime> TimeSource { get; set; }

        private static object trimLock = new object();
        private static Dictionary<Type, List<System.Reflection.PropertyInfo>> PropertiesToTrimCache = [];
        private DateTime Timestamp
        {
            get
            {
                if (TimeSource != null)
                {
                    return TimeSource();
                }
                else
                {
                    return DateTime.UtcNow;
                }
            }
        }

        public FContext(DbContextOptions opts) : base(opts)
        {
        }

        //public FContext(DbContextOptions opts, Type type) : base(opts)
        //{
        //}

        public static FContext GetContext()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new ArgumentException("Connection string has not been set");
            }
            return GetContext(ConnectionString);
        }
        public static FContext GetContext(string connectionString)
        {
            var optsBuilder = new DbContextOptionsBuilder<FContext>();
            optsBuilder.UseSqlServer(connectionString);
            return new FContext(optsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var interfaceType = typeof(IFluentModel);
            var fluentModels = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(i => interfaceType.IsAssignableFrom(i) && i != interfaceType);

            foreach (var fM in fluentModels)
            {
                ((IFluentModel)fM.GetConstructor([]).Invoke(null)).OnModelCreating(modelBuilder);
            }

            //IdentityModelCreating(modelBuilder);
        }

        private void IdentityModelCreating(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);
            modelBuilder.Entity("DungeonMasterToolkit.Web.Data.ApplicationUser", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("nvarchar(450)");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("int");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("bit");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("bit");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("bit");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex")
                    .HasFilter("[NormalizedUserName] IS NOT NULL");

                b.ToTable("AspNetUsers", (string)null);
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex")
                    .HasFilter("[NormalizedName] IS NOT NULL");

                b.ToTable("AspNetRoles", (string)null);
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims", (string)null);
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims", (string)null);
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderKey")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins", (string)null);
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("RoleId")
                    .HasColumnType("nvarchar(450)");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Value")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens", (string)null);
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("DungeonMasterToolkit.Web.Data.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("DungeonMasterToolkit.Web.Data.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("DungeonMasterToolkit.Web.Data.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("DungeonMasterToolkit.Web.Data.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
#pragma warning restore 612, 618
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (false == string.IsNullOrWhiteSpace(ConnectionString))
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }

            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseLoggerFactory(Common.Logging.FLoggingProvider.Instance);
#if DEBUG
            optionsBuilder.ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
#endif

            //base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            if (UseHistTables)
            {
                AddDeletedToHist();
            }

            TrimStringFieldsToLength();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (UseHistTables)
            {
                AddDeletedToHist();
            }
            TrimStringFieldsToLength();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void OpenConnection()
        {
            this.Database.OpenConnection();
        }

        public void CloseConenction()
        {
            this.Database.CloseConnection();
        }

        public async Task OpenConnectionAsync()
        {
            await this.Database.OpenConnectionAsync();
        }

        public async Task CloseConnectionAsync()
        {
            await this.Database.CloseConnectionAsync();
        }

        public int ExecuteSQLCommand(string command, params object[] parameters)
        {
            return this.Database.ExecuteSqlRaw(command, parameters);
        }
        public async Task<int> ExecuteSQLCommandAsync(string command, params object[] parameters)
        {
            return await this.Database.ExecuteSqlRawAsync(command, parameters);
        }

        public void ClearChanges()
        {
            base.ChangeTracker.Clear();
        }
        
        private void AddDeletedToHist()
        {
            foreach (var stateEntry in ChangeTracker.Entries().Where(e => (e.State) == EntityState.Deleted))
            {
                var nS = stateEntry.Entity.GetType().BaseType.Namespace;
                var name = stateEntry.Entity.GetType().BaseType.Name;
                var histType = Type.GetType(string.Format($"{nS}.{name}_HIST"));
                if (histType != null)
                {
                    var hist = Activator.CreateInstance(histType);
                    Common.Utilities.Utils.ReflectionCopy(stateEntry, hist);
                    var propertyInfo = histType.GetProperties()
                        .Where(pi => pi.Name == "Deleted"
                        && pi.PropertyType == typeof(DateTime?)
                        && pi.CanWrite).FirstOrDefault();
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(hist, Timestamp, null);

                        var t = this.GetType();
                        var prop = t.GetProperty(histType.Name);
                        var dbSet = prop.GetValue(this);
                        dbSet.GetType().GetMethod("Add").Invoke(dbSet, [hist]);
                    }
                }
            }
        }

        private void TrimStringFieldsToLength()
        {
            var stateEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (var stateEntry in stateEntries)
            {
                var t = stateEntry.Entity.GetType();
                lock (trimLock)
                {
                    if (false == PropertiesToTrimCache.ContainsKey(t))
                    {
                        PropertiesToTrimCache[t] = new List<System.Reflection.PropertyInfo>();
                        foreach (var prop in t.GetProperties())
                        {
                            if (prop.PropertyType == typeof(string))
                            {
                                var attrs = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute), true);
                                if (attrs.Length > 0)
                                {
                                    PropertiesToTrimCache[t].Add(prop);
                                }
                            }
                        }
                    }
                }

                foreach (var prop in PropertiesToTrimCache[t])
                {
                    var attrs = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute), true);
                    var attr = (System.ComponentModel.DataAnnotations.MaxLengthAttribute)attrs[0];

                    var value = (string)prop.GetValue(stateEntry.Entity);
                    if (value != null && value.Length > attr.Length)
                    {
                        var name = t.Name;
                        if (t.BaseType != typeof(object))
                        {
                            name = t.BaseType.Name;
                        }

                        prop.SetValue(stateEntry.Entity, value.Substring(0, attr.Length));
                    }
                }
            }
        }
    }
}
