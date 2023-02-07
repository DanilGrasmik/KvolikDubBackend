namespace KvolikDubBackend.Models.Dtos;

public class ProfileInfoDto
{
    public Guid Id { get; set; }
    
    public String email { get; set; }
    
    public String? name { get; set; }
    
    public String avatarImageUrl { get; set; }
    
    public bool isAdmin { get; set; }
}