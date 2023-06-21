using Communications.Core.Interfaces;
using Communications.Core.Models;
using SharedLib.ExceptionsHandler.CustomExceptions;

namespace Communications.Application.Notifications;

public class NotificationsHandlerBase : HandlerBase<Notification>
{
    public NotificationsHandlerBase(IUnitOfWork unitOfWork) : base(unitOfWork)
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