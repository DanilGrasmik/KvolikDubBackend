using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Dtos;

public class ReviewDto
{
    [Required] 
    public Guid id { get; set; }
    
    [Required]
    public String reviewText { get; set; }
}