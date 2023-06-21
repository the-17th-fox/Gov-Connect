using Microsoft.AspNetCore.Http;
using SharedLib.ExceptionsHandler.CustomExceptions;
using System.Net;
using System.Text.Json;

namespace SharedLib.ExceptionsHandler;

public class GlobalExceptionsHandler
{
    private readonly RequestDelegate _next;

    private readonly List<ExceptionAndStatusPair> _exceptions;

    public GlobalExceptionsHandler(RequestDelegate next, params ExceptionAndStatusPair[] extraExceptionsTypes)
    {
        _next = next;

        _exceptions = AssignExceptions(extraExceptionsTypes);
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = ExceptionsSwitch(typeof(Exception));

            var exceptionResponse = new
            {
                exceptionType = exception.GetType().Name,
                statusCode = response.StatusCode,
                message = exception.Message,
            };

            var result = JsonSerializer.Serialize(exceptionResponse);
            await response.WriteAsync(result);
        }
    }

    private static List<ExceptionAndStatusPair> AssignExceptions(params ExceptionAndStatusPair[] extraExceptionsTypes)
    {
        var exceptions = new List<ExceptionAndStatusPair>()
        {
            new(typeof(NotFoundException), (int)HttpStatusCode.NotFound),
            new(typeof(BadRequestException), (int)HttpStatusCode.BadRequest),
            new(typeof(AlreadyExistsException), (int)HttpStatusCode.BadRequest)
        };

        if (extraExceptionsTypes.Length > 0)
        {
            exceptions.AddRange(extraExceptionsTypes);
        }

        return exceptions;
    }

    private int ExceptionsSwitch(Type exceptionType)
    {
        ExceptionAndStatusPair? exception = _exceptions.FirstOrDefault(ep => ep.ExceptionType == exceptionType);

        if (exception.HasValue)
        {
            return exception.Value.ExceptionStatusCode;
        }

        return (int)HttpStatusCode.InternalServerError;
    }
}