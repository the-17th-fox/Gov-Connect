using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace SharedLib.ElasticSearch.Middlewares;

public static class CreateIndexesMiddleware
{
    public static async Task<WebApplication> CreateIndexesAsync(this WebApplication app, params ElasticSearchIndexTemplate[] indexesParameters)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var elasticClient = scope.ServiceProvider.GetRequiredService<IElasticClient>();

        foreach (var indexParameters in indexesParameters)
        {
            var indexName = indexParameters.IndexName;
            var indexMessageType = indexParameters.MessageType;

            if (await CheckIfExistsAsync(elasticClient, indexName))
            {
                continue;
            }

            var createIndexResponse = await CreateIndexAsync(elasticClient, indexName, indexMessageType);
            if (!createIndexResponse.IsValid)
            {
                throw new ElasticsearchClientException("Index creation has failed.");
            }
        }

        return app;
    }

    private static async Task<bool> CheckIfExistsAsync(IElasticClient elasticClient, string indexName)
    {
        return (await elasticClient.Indices.ExistsAsync(indexName)).Exists;
    }

    private static async Task<CreateIndexResponse> CreateIndexAsync(
        IElasticClient elasticClient, 
        string indexName,
        Type messageType)
    {
        var createIndexResponse = await elasticClient.Indices.CreateAsync(
            indexName, 
            opt => opt
                .Map(mapOpt => mapOpt.AutoMap(messageType)));

        return createIndexResponse;
    }
}