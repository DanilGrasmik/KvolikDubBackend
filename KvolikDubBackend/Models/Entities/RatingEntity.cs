using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Entities;

public class RatingEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public int Grade { get; set; }
    
    [Required]
    public AnimeEntity Anime { get; set; }
    
    [Required]
    public UserEntity User { get; set; }
}