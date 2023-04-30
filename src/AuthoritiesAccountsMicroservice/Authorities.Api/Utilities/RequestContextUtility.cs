namespace Authorities.Api.Utilities;

public static class RequestContextUtility
{
    public static string GetValueFromHeader(this HttpContext context, string header)
    {
        var keyValuePair = context.Request.Headers
            .FirstOrDefault(h => h.Key == header);

        if (keyValuePair.Key == null)
        {
            throw new ArgumentNullException(nameof(keyValuePair));
        }

        var value = keyValuePair.Value.ToString();

        return value;
    }
}