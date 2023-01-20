using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Dtos;

public class LoginCredentials
{
    [Required(ErrorMessage = "username cant be null")]
    [Range(2, 25, ErrorMessage = "Username length must be in range 2 to 25")]
    public String username { get; set; }
    
    [Required(ErrorMessage = "password cant be null")]
    [Range(6, 30, ErrorMessage = "Password length must be in range 6 to 30")]
    public String password { get; set; }
}