using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Notifications.Queries;

public class GetNotificationByIdQueryHandler 
    : BaseNotificationsHandler, IRequestHandler<GetNotificationByIdQuery, Notification>
{
    public GetNotificationByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Notification> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var notification = await GetIfExistsAsync(request.Id);

        return notification;
    }
}