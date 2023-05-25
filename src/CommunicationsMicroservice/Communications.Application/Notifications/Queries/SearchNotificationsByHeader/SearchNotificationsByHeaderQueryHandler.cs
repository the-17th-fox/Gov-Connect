using Communications.Application.Utilities;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;

namespace Communications.Application.Notifications.Queries;

public class SearchNotificationsByHeaderQueryHandler : NotificationsHandlerBase, IRequestHandler<SearchNotificationsByHeaderQuery, List<IndexedMessageViewModel>>
{
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _notificationsIndexName;
    private readonly byte _queriedElementsAmount;

    public SearchNotificationsByHeaderQueryHandler(
        IUnitOfWork unitOfWork, 
        IElasticClient elasticSearchClient,
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _notificationsIndexName = elasticSearchIndexesOptions.Value.NotificationsIndexName;
        _queriedElementsAmount = elasticSearchIndexesOptions.Value.QueriedElementsAmount;
    }

    public async Task<List<IndexedMessageViewModel>> Handle(SearchNotificationsByHeaderQuery request, CancellationToken cancellationToken)
    {
        var searchResponse =
            await _elasticSearchClient.GetIndexedDataAsync(request.Query, _notificationsIndexName, _queriedElementsAmount);

        return searchResponse.Hits
            .Select(h => h.Source)
            .ToList();
    }
}