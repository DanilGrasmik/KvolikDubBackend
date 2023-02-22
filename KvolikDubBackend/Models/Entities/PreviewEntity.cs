namespace KvolikDubBackend.Models.Entities;

public class PreviewEntity
{
    public Guid Id { get; set; }
    
    public string ShortName { get; set; }
    
    public string Name { get; set; }
    
    public int AgeLimit { get; set; }
    
    public String Type { get; set; }
    
    public String? Description { get; set; }
    
    public DateTime? ReleaseFrom { get; set; }
}