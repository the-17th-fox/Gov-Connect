using Communications.Application.ViewModels.ElasticSearch;
using SharedLib.ElasticSearch.Middlewares;

namespace Communications.Api.Configuration;

public static class AppConfiguration
{
    public static async Task<WebApplication?> ConfigureElasticIndexes(this WebApplication app, IConfiguration configuration, string elasticSearchSectionPath)
    {
        var elasticSection = configuration.GetSection(elasticSearchSectionPath);

        if (!elasticSection.GetValue<bool>("IsEnabled"))
        {
            return app;
        }

        await app.CreateIndexesAsync(
            new(elasticSection.GetValue<string>("ReportsIndexName"), typeof(IndexedMessageViewModel)),
            new(elasticSection.GetValue<string>("NotificationsIndexName"), typeof(IndexedMessageViewModel)));

        return app;
    }
}