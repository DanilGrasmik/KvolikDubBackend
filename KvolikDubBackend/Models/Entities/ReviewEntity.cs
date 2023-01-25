using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Entities;

public class ReviewEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public String ReviewText { get; set; }

    [Required]
    public DateTime PublishTime { get; set; }
    
    [Required]
    public AnimeEntity Anime { get; set; }
    
    [Required]
    public UserEntity User { get; set; }
}