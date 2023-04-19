using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Notifications.Queries.GetAllNotifications;

public class GetAllNotificationsQueryHandler 
    : BaseNotificationsHandler, IRequestHandler<GetAllNotificationsQuery, List<Notification>>
{
    public GetAllNotificationsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<List<Notification>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.NotificationsRepository.GetAllAsync(request.PageNumber, request.PageSize);
    }
}