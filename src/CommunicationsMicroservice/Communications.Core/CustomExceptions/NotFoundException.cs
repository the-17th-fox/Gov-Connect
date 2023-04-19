namespace Communications.Core.CustomExceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message = "Requested object with the specified identifier hasn't been found.") 
        : base(message) { }
}