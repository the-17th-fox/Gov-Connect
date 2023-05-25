using Communications.Application.Utilities;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;
using SharedLib.ElasticSearch.Extensions;

namespace Communications.Application.Notifications.Commands;

public class UpdateNotificationCommandHandler : NotificationsHandlerBase, IRequestHandler<UpdateNotificationCommand>
{
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _notificationsIndexName;

    public UpdateNotificationCommandHandler(
        IUnitOfWork unitOfWork,
        IElasticClient elasticSearchClient,
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _notificationsIndexName = elasticSearchIndexesOptions.Value.NotificationsIndexName;
    }

    public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await GetIfExistsAsync(request.Id);

        notification.Header = request.Header;
        notification.Body = request.Body;

        notification.Classifications.Clear();
        foreach (var classificationId in request.ClassificationsIds)
        {
            var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(classificationId);
            if (classification != null)
            {
                notification.Classifications.Add(classification);
            }
        }

        UnitOfWork.NotificationsRepository.Update(notification);

        await UnitOfWork.SaveChangesAsync();
        await IndexNotificationsAsync(notification.Id, notification.Header, notification.CreatedAt);
    }

    private async Task IndexNotificationsAsync(Guid id, string header, DateTime createdAt)
    {
        var indexedMessageViewModel = new IndexedMessageViewModel(id, header, createdAt);
        await _elasticSearchClient.IndexDataAsync(_notificationsIndexName, indexedMessageViewModel);
    }
}