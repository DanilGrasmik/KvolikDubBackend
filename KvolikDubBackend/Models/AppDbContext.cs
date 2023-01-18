using KvolikDubBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Models;

public class AppDbContext : DbContext
{
    public DbSet<AnimeEntity> Animes { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }
}