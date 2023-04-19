namespace Communications.Core.CustomExceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) {}
}