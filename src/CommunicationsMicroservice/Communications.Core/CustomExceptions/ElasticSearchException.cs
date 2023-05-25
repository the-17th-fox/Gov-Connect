namespace Communications.Core.CustomExceptions;

public class ElasticSearchException : Exception
{
    public ElasticSearchException(string message = "Search response is invalid.") : base(message)
    {
        
    }
}