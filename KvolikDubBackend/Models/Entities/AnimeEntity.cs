using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Enums;

namespace KvolikDubBackend.Models.Entities;

public class AnimeEntity
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public String ShortName { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Name min length is 1")]
    public String Name { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "NameEng min length is 1")]
    public String NameEng { get; set; }
    
    public String Type { get; set; }
    
    public int EpisodesAmount { get; set; }

    public List<String> Genres { get; set; } = new();
    
    public PrimarySource? PrimarySource { get; set; }
    
    public DateTime? ReleaseFrom { get; set; }

    public DateTime? ReleaseBy { get; set; }
    
    [Required]
    [Range(0, 21, ErrorMessage = "Range for ageLimit is 0 to 21")]
    public int AgeLimit { get; set; }
    
    public int Duration { get; set; }
    
    public String? Description { get; set; }
    
    public ExitStatus ExitStatus { get; set; }
    
    public List<String> Frames { get; set; } = new();
    
    public String ImageUrl { get; set; }
    
    public String? TrailerUrl { get; set; }
    
    public VoiceoverStatus VoiceoverStatus { get; set; }

    //public List<RatingEntity> Ratings { get; set; } = new();

    //public List<UserEntity> LikedUsers { get; set; } = new();
}