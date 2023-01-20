using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    [Required]
    [Range(2, 25, ErrorMessage = "Username length must be in range 2 to 25")]
    public String Username { get; set; }
    
    [Required]
    public String HashedPassword { get; set; }
    
    public String? Name { get; set; }
    
    [Required]
    public String AvatarImageUrl { get; set; }
    
    [Required]
    public bool IsAdmin { get; set; }
    
    public List<AnimeEntity> FavoriteAnimes { get; set; } = new();

    //public List<RatingEntity> Ratings { get; set; } = new();

    //public List<ReviewEntity> Reviews { get; set; } = new();
}