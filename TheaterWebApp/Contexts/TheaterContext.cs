using Microsoft.EntityFrameworkCore;
using TheaterWebApp.Entities;

namespace TheaterWebApp.Contexts;

public class TheaterContext : DbContext
{

    public DbSet<Movie> Movies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<MovieImage> MovieImages { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<MovieRating> MovieRatings { get; set; }

    public TheaterContext(DbContextOptions<TheaterContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("movies")
                .HasKey(m => m.Id);

            entity.Property(m => m.Title)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(m => m.Description)
                .HasMaxLength(5000);

            entity.HasMany(m => m.MovieImages)
                .WithOne(mi => mi.Movie)
                .HasForeignKey(m => m.MovieId);

            entity.HasMany(m => m.MovieGenres)
                .WithOne(m => m.Movie)
                .HasForeignKey(m => m.MovieId);

            entity.HasMany(m => m.MovieRatings)
                .WithOne(m => m.Movie)
                .HasForeignKey(m => m.MovieId);

            entity.HasMany(m => m.Comments)
                .WithOne(m => m.Movie)
                .HasForeignKey(m => m.MovieId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users")
                .HasKey(m => m.Id);

            entity.Property(u => u.Email)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(u => u.Nickname)
                .HasMaxLength(100);

            entity.Property(u => u.Password)
                .HasMaxLength(1000)
                .IsRequired();

            entity.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("comments")
                .HasKey(c => c.Id);

            entity.Property(c => c.Content)
                .HasMaxLength(2000)
                .IsRequired();

            entity.HasMany(c => c.NestedComments)
                .WithOne(nc => nc.ParentComment)
                .HasForeignKey(nc => nc.ParentCommendId);
        });

        modelBuilder.Entity<MovieImage>()
            .ToTable("movie_images");

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.ToTable("movie_genres")
                .HasKey(g => g.Id);

            entity.Property(mg => mg.Genre)
                .HasConversion<string>();
        });

        modelBuilder.Entity<MovieRating>(entity =>
        {
            entity.ToTable("movie_ratings")
                .HasKey(mr => mr.Id);

            // 한 유저가 하나의 영화에 대해 한 번만 평점을 줄 수 있도록 유니크 인덱스 설정
            entity.HasIndex(r => new { r.UserId, r.MovieId })
                .IsUnique();
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}