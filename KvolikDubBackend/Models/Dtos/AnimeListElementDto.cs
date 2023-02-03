using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Enums;

namespace KvolikDubBackend.Models.Dtos;

public class AnimeListElementDto
{
    [Required]
    public Guid id { get; set; }
    
    public string shortName { get; set; }

    [MinLength(1, ErrorMessage = "name min length is 1")]
    public String name { get; set; }
    
    [MinLength(1, ErrorMessage = "nameEng min length is 1")]
    public String nameEng { get; set; }
    
    public String type { get; set; }
    
    public DateTime? releaseFrom { get; set; }
    
    public String imageUrl { get; set; }
    
    public int episodesAmount { get; set; }
    
    public double? averageRating { get; set; }
}