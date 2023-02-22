using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Dtos;

public class LoginCredentials
{
    [Required(ErrorMessage = "username cant be null")]
    public String email { get; set; }
    
    [Required(ErrorMessage = "password cant be null")]
    [Range(6, 30, ErrorMessage = "Password length must be in range 6 to 30")]
    public String password { get; set; }
}