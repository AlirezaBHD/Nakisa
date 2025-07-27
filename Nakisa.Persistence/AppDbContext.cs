using Microsoft.EntityFrameworkCore;
using Nakisa.Domain.Entities;

namespace Nakisa.Persistence;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.ModifiedOn = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Playlist>()
            .HasOne(sd => sd.Category)
            .WithMany(a => a.Playlists)
            .HasForeignKey(sd => sd.CategoryId);
        
        modelBuilder.Entity<Playlist>()
            .HasOne(p => p.Parent)
            .WithMany(p => p.SubPlaylists)
            .HasForeignKey(p => p.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Music>()
            .HasOne(sd => sd.Playlist)
            .WithMany(a => a.Musics)
            .HasForeignKey(sd => sd.PlaylistId);
        
        modelBuilder.Entity<Music>()
            .HasOne(sd => sd.PostedBy)
            .WithMany(a => a.PostedMusics)
            .HasForeignKey(sd => sd.UserId);
        
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<Music> Tracks => Set<Music>();
}