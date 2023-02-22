namespace KvolikDubBackend.Models.Dtos;

public class CreateAvatarDto
{
    public string x { get; set; }
    public List<IFormFile> imageUris { get; set; }
}