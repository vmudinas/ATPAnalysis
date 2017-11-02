using Infrastructure.Entities;
using Infrastructure.UserModel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataContext
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationField> LocationFields { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<SiteUser> SiteUsers { get; set; }
        public DbSet<SureTrendVersionInfo> SureTrendVersionInfos { get; set; }
        public DbSet<Surface> Surfaces { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanLocation> PlanLocations { get; set; }
        public DbSet<PlanUnit> PlanUnits { get; set; }
        public DbSet<PlanHistory> PlanHistories { get; set; }
        public DbSet<PlanHistoryResult> PlanHistoryResults { get; set; }
        public DbSet<LanguageResource> LanguageResources { get; set; }
        public DbSet<LanguageValue> LanguageValues { get; set; }
        public DbSet<LanguageUserValue> LanguageUserValues { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<RegisterUnitToken> UnitRegisterTokens { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UserFunctionRole> UsersRoles { get; set; }
        //  public DbSet<ApplicationRole> Role { get; set; }
        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
        // public DbSet<ApplicationUserExtention> ApplicationUser { get; set; }

        public DbSet<ReportSchedule> ReportSchedules { get; set; }

        public DbSet<ReportScheduleSite> ReportScheduleSites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //    builder.UseSqlServer("Data Source=DEV-PC1\\SQLEXPRESS;Initial Catalog=Hygiena2;Integrated Security=True");
          // builder.UseSqlServer(
            //   "Server=tcp:zvp4bnjcrg.database.windows.net,1433;Initial Catalog=SureTrendDev;Persist Security Info=False;User ID=hygiena;Password=SystemSURE2$#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
           // builder.UseSqlite("Data Source=hygienaLiteDb.db");
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //Adding Composite Keys  

            builder.Entity<LocationField>().HasKey(k => new {k.AccountId, k.FieldId, k.DeviceUsageId});
            builder.Entity<Site>().HasKey(k => new {k.SiteId});
            builder.Entity<Site>().Property(x => x.Active).HasDefaultValue(true);
            builder.Entity<UnitLocationMap>().HasKey(k => new {k.UnitNo, k.LocationId});
            builder.Entity<LanguageResource>(b => { b.HasIndex(p => p.LogicName).IsUnique(); });
        }
    }
}