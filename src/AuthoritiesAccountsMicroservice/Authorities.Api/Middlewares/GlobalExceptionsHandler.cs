using System.Net;
using System.Text.Json;
using Exception = System.Exception;

namespace Authorities.Api.Middlewares
{
    public class GlobalExceptionsHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionsHandler(RequestDelegate next)
        {
            _next = next;
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

                response.StatusCode = exception switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,

                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    
                    _ => (int)HttpStatusCode.InternalServerError
                };

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
    }
}
