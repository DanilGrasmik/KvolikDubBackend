using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Dtos;

public class UserRegisterDto
{
    [Required(ErrorMessage = "username field is required")]
    [Range(2, 25, ErrorMessage = "Username length must be in range 2 to 25")]
    public String username { get; set; }
    
    [Required(ErrorMessage = "password field is required")]
    [Range(6, 30, ErrorMessage = "Password length must be in range 6 to 30")]
    public String password { get; set; }
    
    public String? name { get; set; }
}