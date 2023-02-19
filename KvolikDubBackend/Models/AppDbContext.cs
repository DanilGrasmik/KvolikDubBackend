using KvolikDubBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Models;

public class AppDbContext : DbContext
{
    public DbSet<AnimeEntity> Animes { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TokenEntity> Tokens { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<RatingEntity> Ratings { get; set; }
    public DbSet<BadWordEntity> BadWords { get; set; }
    public DbSet<ConfirmCodeEntity> ConfirmCodes { get; set; }
    public DbSet<AvatarEntity> Avatars { get; set; }
    public DbSet<PreviewEntity> Previews { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }
}