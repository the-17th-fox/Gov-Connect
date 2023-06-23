using Microsoft.AspNetCore.Http;
using SharedLib.ExceptionsHandler.CustomExceptions;
using System.Net;
using System.Text.Json;

namespace SharedLib.ExceptionsHandler;

public class GlobalExceptionsHandler
{
    private readonly RequestDelegate _next;

    private readonly List<ExceptionAndStatusPair> _exceptions = new()
    {
        new(typeof(NotFoundException), (int)HttpStatusCode.NotFound),
        new(typeof(BadRequestException), (int)HttpStatusCode.BadRequest),
        new(typeof(AlreadyExistsException), (int)HttpStatusCode.BadRequest)
    };

    public GlobalExceptionsHandler(RequestDelegate next, ICollection<ExceptionAndStatusPair>? additionalExceptions)
    {
        _next = next;

        AssignExceptions(additionalExceptions);
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

    private void AssignExceptions(in ICollection<ExceptionAndStatusPair>? extraExceptionsTypes)
    {
        if (extraExceptionsTypes != null && extraExceptionsTypes.Count > 0)
        {
            _exceptions.AddRange(extraExceptionsTypes);
        }
    }

    private int ExceptionsSwitch(Type exceptionType)
    {
        var exists = _exceptions.Exists(ep => ep.ExceptionType == exceptionType);

        if (exists)
        {
            return _exceptions.Find(ep => ep.ExceptionType == exceptionType)
                .ExceptionStatusCode;
        }

        return (int)HttpStatusCode.InternalServerError;
    }
}