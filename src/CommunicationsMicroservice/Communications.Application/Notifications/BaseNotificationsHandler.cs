using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;

namespace Communications.Application.Notifications;

public class BaseNotificationsHandler : HandlerBase<Notification>
{
    public BaseNotificationsHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected override async Task<Notification> GetIfExistsAsync(Guid id)
    {
        var notification = await UnitOfWork.NotificationsRepository.GetByIdAsync(id);
        if (notification == null)
        {
            throw new NotFoundException();
        }

        return notification;
    }
}