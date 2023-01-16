using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Dtos;

public class AnimeListElementDto
{
    [Required]
    public Guid id { get; set; }
    
    [MinLength(1, ErrorMessage = "name min length is 1")]
    public String name { get; set; }
    
    [MinLength(1, ErrorMessage = "nameEng min length is 1")]
    public String nameEng { get; set; }
    
    public List<String> genres { get; set; } = new ();
    
    [Range(0, 21, ErrorMessage = "Range for ageLimit is 0 to 21")]
    public int ageLimit { get; set; }
    
    public String type { get; set; }
    
    public int duration { get; set; }
    
    public int episodesAmount { get; set; }
    
    public String description { get; set; }
    
    //TODO: maybe not string 
    public String imageUrl { get; set; }
}