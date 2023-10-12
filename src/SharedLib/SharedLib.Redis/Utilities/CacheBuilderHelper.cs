using Microsoft.AspNetCore.Http;
using System.Text;

namespace SharedLib.Redis.Utilities;

internal static class CacheBuilderHelper
{
    internal static string GenerateCacheKey(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        if (request.Query.Count > 0)
        {
            AppendQueryToKeyBuilder(keyBuilder, request.Query);
        }

        return keyBuilder.ToString();
    }

    private static void AppendQueryToKeyBuilder(StringBuilder keyBuilder, IQueryCollection query)
    {
        keyBuilder.Append("|QUERY:");
        foreach (var (key, value) in query.OrderBy(p => p.Key))
        {
            keyBuilder.Append($"[{key}]-[{value}]|");
        }
    }
}