using LandmarkEmulator.Database.Character.Model;
using LandmarkEmulator.Database.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LandmarkEmulator.Database.Character
{
    public class CharacterContext : DbContext
    {
        public DbSet<CharacterModel> Character { get; set; }
        public DbSet<CharacterCustomisationModel> CharacterCustomisation { get; set; }

        private readonly IDatabaseConfig config;

        public CharacterContext(IDatabaseConfig config)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseConfiguration(config, DatabaseType.Character);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterModel>(entity =>
            {
                entity.ToTable("character");

                entity.HasIndex(e => e.AccountId)
                    .HasDatabaseName("accountId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.AccountId)
                    .HasColumnName("accountId")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("deleteTime")
                    .HasColumnType("datetime")
                    .HasDefaultValue(null);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.LastOnline)
                    .HasColumnName("lastOnline")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastServerId)
                    .HasColumnName("lastServerId")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValue("");

                entity.Property(e => e.OriginalName)
                    .HasColumnName("originalName")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValue(null);

                entity.Property(e => e.ProfileTypeId)
                    .HasColumnName("profileTypeId")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.Race)
                    .HasColumnName("race")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.SkinTint)
                    .HasColumnName("skinTint")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<CharacterCustomisationModel>(entity =>
            {
                entity.ToTable("character_customisation");

                entity.HasKey(e => new { e.Id, e.Slot })
                    .HasName("PRIMARY");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.Slot)
                    .HasColumnName("slot")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.Option)
                    .HasColumnName("option")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValue(0);

                entity.Property(e => e.Tint)
                    .HasColumnName("tint")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValue(0);

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Customisation)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK__character_customisation_id__character_id");
            });
        }
    }
}
