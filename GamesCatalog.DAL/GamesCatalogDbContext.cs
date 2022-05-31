using EntityFramework.Exceptions.SqlServer;

using GamesCatalog.DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace GamesCatalogAPI.Models
{
    public partial class GamesCatalogDbContext : DbContext
    {
        public GamesCatalogDbContext()
        {
        }


        public GamesCatalogDbContext(DbContextOptions<GamesCatalogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Developer> Developers { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Developer>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(36);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(36);

                entity.HasOne(d => d.Developer)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.DeveloperId)
                    .HasConstraintName("Games_FK");

                entity.HasMany(d => d.Genres)
                    .WithMany(p => p.Games)
                    .UsingEntity<Dictionary<string, object>>(
                        "GamesGenre",
                        l => l.HasOne<Genre>().WithMany().HasForeignKey("GenresId").OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("GamesGenresGenres_FK"),
                        r => r.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("GamesGenresGames_FK"),
                        j =>
                        {
                            j.HasKey("GameId", "GenresId").HasName("GamesGenres_PK");

                            j.ToTable("GamesGenres");
                        });
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name).HasMaxLength(25);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
