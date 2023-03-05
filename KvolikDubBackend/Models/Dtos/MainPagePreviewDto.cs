namespace KvolikDubBackend.Models.Dtos;

public class MainPagePreviewDto
{
    public String shortName { get; set; }
    
    public String name { get; set; }
    
    public DateTime? releaseFrom { get; set; }
    
    public int ageLimit { get; set; }
    
    public String type { get; set; }
    
    public String? description { get; set; }

    public String? previewVideoUrl { get; set; }
}