namespace KvolikDubBackend.Services.Interfaces;

public interface IEmailService
{
    public void SendCodeToEmail(String emailAddress);
}