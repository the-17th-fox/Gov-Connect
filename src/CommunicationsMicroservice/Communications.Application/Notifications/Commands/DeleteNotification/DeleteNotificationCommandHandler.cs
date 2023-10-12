using Communications.Application.Utilities;
using Communications.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;
using SharedLib.ElasticSearch.Extensions;

namespace Communications.Application.Notifications.Commands;

public class DeleteNotificationCommandHandler : NotificationsHandlerBase, IRequestHandler<DeleteNotificationCommand>
{
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _notificationsIndexName;

    public DeleteNotificationCommandHandler(
        IUnitOfWork unitOfWork, 
        IElasticClient elasticSearchClient,
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _notificationsIndexName = elasticSearchIndexesOptions.Value.NotificationsIndexName;
    }

    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await GetIfExistsAsync(request.Id);

        UnitOfWork.NotificationsRepository.Delete(notification);

        await UnitOfWork.SaveChangesAsync();
        await _elasticSearchClient.DeleteIndexedDataAsync(_notificationsIndexName, notification.Id);
    }
}