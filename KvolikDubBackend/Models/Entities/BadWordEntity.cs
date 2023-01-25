using System.ComponentModel.DataAnnotations;

namespace KvolikDubBackend.Models.Entities;

public class BadWordEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public String Word { get; set; }
}