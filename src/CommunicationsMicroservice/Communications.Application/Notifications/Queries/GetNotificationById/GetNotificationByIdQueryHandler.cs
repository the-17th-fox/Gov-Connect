using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Notifications.Queries;

public class GetNotificationByIdQueryHandler 
    : NotificationsHandlerBase, IRequestHandler<GetNotificationByIdQuery, Notification>
{
    public GetNotificationByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Notification> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        return await GetIfExistsAsync(request.Id);
    }
}