using AutoMapper;
using Communications.Application.Utilities;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;
using SharedLib.ElasticSearch.Extensions;

namespace Communications.Application.Notifications.Commands;

public class CreateNotificationCommandHandler : NotificationsHandlerBase, IRequestHandler<CreateNotificationCommand>
{
    private readonly IMapper _mapper;
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _notificationsIndexName;

    public CreateNotificationCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IElasticClient elasticSearchClient,
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _notificationsIndexName = elasticSearchIndexesOptions.Value.NotificationsIndexName;
    }

    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = _mapper.Map<Notification>(request);

        foreach (var classificationId in request.ClassificationsIds)
        {
            var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(classificationId);
            if (classification != null)
            {
                notification.Classifications.Add(classification);
            }
        }

        UnitOfWork.NotificationsRepository.Create(notification);

        await UnitOfWork.SaveChangesAsync();
        await IndexNotificationsAsync(notification.Id, notification.Header, notification.CreatedAt);
    }

    private async Task IndexNotificationsAsync(Guid id, string header, DateTime createdAt)
    {
        var indexedMessageViewModel = new IndexedMessageViewModel(id, header, createdAt);
        await _elasticSearchClient.IndexDataAsync(_notificationsIndexName, indexedMessageViewModel);
    }
}