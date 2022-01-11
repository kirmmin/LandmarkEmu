using LandmarkEmulator.Database.Auth.Model;
using LandmarkEmulator.Database.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LandmarkEmulator.Database.Auth
{
    public class AuthContext : DbContext
    {
        public DbSet<AccountModel> Account { get; set; }
        public DbSet<ZoneServerModel> ZoneServer { get; set; }

        private readonly IDatabaseConfig config;

        public AuthContext(IDatabaseConfig config)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseConfiguration(config, DatabaseType.Auth);

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.Username)
                    .HasDatabaseName("username");

                entity.HasIndex(e => e.Salt)
                    .HasDatabaseName("salt");

                entity.HasIndex(e => e.Verifier)
                    .HasDatabaseName("verifier");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(128")
                    .HasDefaultValue("");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("s")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValue("");

                entity.Property(e => e.Verifier)
                    .IsRequired()
                    .HasColumnName("v")
                    .HasColumnType("varchar(512)")
                    .HasDefaultValue("");

                entity.Property(e => e.SessionKey)
                    .IsRequired()
                    .HasColumnName("sessionKey")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValue("");

                entity.Property(e => e.SessionKeyExpiration)
                    .HasColumnName("sessionKeyExpiration")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.ServerTicket)
                    .IsRequired()
                    .HasColumnName("serverTicket")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValue("");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp()");
            });

            modelBuilder.Entity<ZoneServerModel>(entity =>
            {
                entity.ToTable("zone_server");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NameId)
                    .IsRequired()
                    .HasColumnName("nameId")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValue(62147);

                entity.Property(e => e.Host)
                    .IsRequired()
                    .HasColumnName("host")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValue("127.0.0.1");

                entity.Property(e => e.Port)
                    .IsRequired()
                    .HasColumnName("port")
                    .HasColumnType("smallint(5) unsigned")
                    .HasDefaultValue(19000);

                entity.Property(e => e.Flags)
                    .HasColumnName("flags")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValue(0);

                entity.HasData(new ZoneServerModel
                {
                    Id     = 1,
                    NameId = 62147,
                    Host   = "127.0.0.1",
                    Port   = 19000,
                    Flags  = 1
                });
            });
        }
    }
}
