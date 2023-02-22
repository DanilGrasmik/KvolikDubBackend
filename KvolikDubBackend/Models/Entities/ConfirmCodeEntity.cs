namespace KvolikDubBackend.Models.Entities;

public class ConfirmCodeEntity
{
    public Guid Id { get; set; }
    
    public string UserEmail { get; set; }
    
    public int Code { get; set; }
    
    public DateTime ExpiredDate { get; set; }

    public int ConfirmAttempts { get; set; } = 0;
}