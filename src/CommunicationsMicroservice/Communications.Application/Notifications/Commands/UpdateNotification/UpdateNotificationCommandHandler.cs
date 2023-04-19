using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Notifications.Commands;

public class UpdateNotificationCommandHandler : BaseNotificationsHandler, IRequestHandler<UpdateNotificationCommand>
{
    public UpdateNotificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await GetIfExistsAsync(request.Id);

        notification.Header = request.Header;
        notification.Body = request.Body;

        UnitOfWork.NotificationsRepository.Update(notification);

        await UnitOfWork.SaveChangesAsync();
    }
}