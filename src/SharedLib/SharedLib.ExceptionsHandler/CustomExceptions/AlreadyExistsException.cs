namespace SharedLib.ExceptionsHandler.CustomExceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string message = "An object with such parameters is already present in the database.") : base(message) { }
}