using Nest;
using SharedLib.ElasticSearch.Interfaces;

namespace SharedLib.ElasticSearch.Extensions;

public static class ElasticSearchExtensions
{
    /// <summary>
    /// Adds or updates a message in the index
    /// </summary>
    /// <param name="elasticClient"></param>
    /// <param name="indexName"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task IndexDataAsync<TMessage>(this IElasticClient elasticClient, string indexName, TMessage message)
        where TMessage : class, IIndexedMessage
    {
        await elasticClient.IndexAsync(new IndexRequest<TMessage>(message, indexName));
    }

    public static async Task DeleteIndexedDataAsync(this IElasticClient elasticClient, string indexName, Guid messageId)
    {
        await elasticClient.DeleteAsync(new DeleteRequest(indexName, messageId));
    }
}