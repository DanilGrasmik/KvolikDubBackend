namespace KvolikDubBackend.Exceptions;

public class GoneException : Exception
{
    public GoneException(string message) : base(message)
    {
    }
}