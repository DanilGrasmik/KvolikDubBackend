using KvolikDubBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Models;

public class AppDbContext : DbContext
{
    public DbSet<AnimeEntity> Animes { get; set; }
    
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<TokenEntity> Tokens { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }
}