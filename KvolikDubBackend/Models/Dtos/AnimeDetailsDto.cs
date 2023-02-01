using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Enums;

namespace KvolikDubBackend.Models.Dtos;

public class AnimeDetailsDto
{
    public Guid id { get; set; }
    
    public string shortName { get; set; }
    
    [MinLength(1, ErrorMessage = "name min length is 1")]
    public String name { get; set; }
    
    [MinLength(1, ErrorMessage = "nameEng min length is 1")]
    public String nameEng { get; set; }
    
    public String type { get; set; }
    
    public int episodesAmount { get; set; }

    public List<String> genres { get; set; } = new();
    
    public PrimarySource? primarySource { get; set; }
    
    public DateTime? releaseFrom { get; set; }

    public DateTime? releaseBy { get; set; }
    
    [Range(0, 21, ErrorMessage = "Range for ageLimit is 0 to 21")]
    public int ageLimit { get; set; }
    
    public int duration { get; set; }
    
    public String? description { get; set; }
    
    public ExitStatus exitStatus { get; set; }
    
    public List<String> frames { get; set; } = new();
    
    public String imageUrl { get; set; }
    
    public String? trailerUrl { get; set; }
    
    public double averageRating { get; set; }
    
    public bool isMonophonic { get; set; }
    
    public VoiceoverStatus voiceoverStatus { get; set; }
    public List<ReviewDto> reviews { get; set; } = new();
}