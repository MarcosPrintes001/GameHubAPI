using Microsoft.EntityFrameworkCore;
using GameHub.API.Models;
using GameHub.API.Services.Auth;

namespace GameHub.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Favorite> Favorites => Set<Favorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Chave composta para favoritos
        modelBuilder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.GameId })
            .IsUnique();
    }



}
