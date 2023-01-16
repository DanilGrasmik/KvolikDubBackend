using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Enums;

namespace KvolikDubBackend.Models.Dtos;

public class CreateAnimeDto
{
    [MinLength(1, ErrorMessage = "name min length is 1")]
    public String name { get; set; }
    
    [MinLength(1, ErrorMessage = "nameEng min length is 1")]
    public String nameEng { get; set; }
    
    public String type { get; set; }
    
    public int episodesAmount { get; set; }

    public List<String> genres { get; set; } = new();
    
    public PrimarySource primarySource { get; set; }
    
    public DateTime? season { get; set; }
    
    public DateTime? realeseFrom { get; set; }

    public DateTime? realeseBy { get; set; }
    
    [Range(0, 21, ErrorMessage = "Range for ageLimit is 0 to 21")]
    public int ageLimit { get; set; }
    
    public int duration { get; set; }
    
    public String description { get; set; }
    
    //TODO: image ???
    public List<String> frames { get; set; } = new();
    
    //TODO: maybe not string 
    public String imageUrl { get; set; }
    
    public String? trailerUrl { get; set; }
}