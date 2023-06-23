namespace SharedLib.ExceptionsHandler;

public struct ExceptionAndStatusPair
{
    /// <summary>
    /// It is better to add only such pairs, which status code isn't InternalServerError. 
    /// All unregistered exceptions returns InternalServerError by default.
    /// </summary>
    /// <param name="exceptionType"></param>
    /// <param name="exceptionStatusCode"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ExceptionAndStatusPair(Type exceptionType, int exceptionStatusCode)
    {
        ExceptionType = exceptionType ?? throw new ArgumentNullException(nameof(exceptionType));
        ExceptionStatusCode = exceptionStatusCode;
    }

    public Type ExceptionType { get; set; }
    public int ExceptionStatusCode { get; set; }


}