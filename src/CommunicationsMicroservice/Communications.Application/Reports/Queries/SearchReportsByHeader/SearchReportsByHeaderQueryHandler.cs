using Communications.Application.Utilities;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;

namespace Communications.Application.Reports.Queries;

public class SearchReportsByHeaderQueryHandler : ReportsHandlerBase, IRequestHandler<SearchReportsByHeaderQuery, List<IndexedMessageViewModel>>
{
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _reportsIndexName;
    private readonly byte _queriedElementsAmount;

    public SearchReportsByHeaderQueryHandler(
        IUnitOfWork unitOfWork, 
        IElasticClient elasticSearchClient,
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _reportsIndexName = elasticSearchIndexesOptions.Value.ReportsIndexName;
        _queriedElementsAmount = elasticSearchIndexesOptions.Value.QueriedElementsAmount;
    }

    public async Task<List<IndexedMessageViewModel>> Handle(SearchReportsByHeaderQuery request, CancellationToken cancellationToken)
    {
        var searchResponse =
            await _elasticSearchClient.GetIndexedDataAsync(request.Query, _reportsIndexName, _queriedElementsAmount);

        return searchResponse.Hits
            .Select(h => h.Source)
            .ToList();
    }
}