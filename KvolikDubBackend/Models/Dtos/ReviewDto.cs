using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Dtos;

public class ReviewDto
{
    [Required] 
    public Guid id { get; set; }
    
    [Required]
    public String name { get; set; }
    
    [Required]
    public String reviewText { get; set; }
    
    [Required]
    public int likes { get; set; }
    
    [Required]
    public String avatarImageUrl { get; set; }

    [Required]
    public DateTime publishTime { get; set; }
}