using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.CustomExceptions;
using Nest;

namespace Communications.Application.Utilities;

public static class ElasticSearchExtensions
{
    public static async Task<ISearchResponse<IndexedMessageViewModel>> GetIndexedDataAsync(
        this IElasticClient elasticClient,
        string query,
        string indexName,
        byte queriedElementsAmount = 10)
    {
        var querySelector = GetSearchDescriptor(query, indexName, queriedElementsAmount);

        var searchResponse = await elasticClient.SearchAsync<IndexedMessageViewModel>(selector: _ => querySelector);

        if (!searchResponse.IsValid)
        {
            throw new ElasticSearchException();
        }

        return searchResponse;
    }

    private static SearchDescriptor<IndexedMessageViewModel> GetSearchDescriptor(string query, string indexName, byte queriedElementsAmount)
    {
        var querySelector = new SearchDescriptor<IndexedMessageViewModel>();

        // Add request query
        querySelector
            .Index(indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Header)
                    .Query(query)
                )
            );

        // Add sorting
        querySelector
            .Sort(sort => sort
                .Descending(o => o.Date)
            )
            .Size(queriedElementsAmount);

        return querySelector;
    }
}