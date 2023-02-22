using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Enums;

namespace KvolikDubBackend.Models.Dtos;

public class CreateAnimeDto
{
    public String shortName { get; set; }
    
    [MinLength(1, ErrorMessage = "name min length is 1")]
    public String name { get; set; }
    
    [MinLength(1, ErrorMessage = "nameEng min length is 1")]
    public String nameEng { get; set; }
    
    public String type { get; set; }
    
    public int episodesAmount { get; set; }

    public List<String> genres { get; set; }
    
    public PrimarySource? primarySource { get; set; }
    
    public DateTime? releaseFrom { get; set; }

    public DateTime? releaseBy { get; set; }
    
    [Range(0, 21, ErrorMessage = "Range for ageLimit is 0 to 21")]
    public int ageLimit { get; set; }
    
    public int duration { get; set; }
    
    public String? description { get; set; }
    
    [Required]
    public ExitStatus exitStatus { get; set; }

    public List<IFormFile> frames { get; set; } = new();
    
    //public String imageUrl { get; set; }
    
    public String? trailerUrl { get; set; }
    
    public String playerLink { get; set; }
    public bool isMonophonic { get; set; }
    
    public IFormFile imageUri { get; set; }
    public VoiceoverStatus voiceoverStatus { get; set; }
}