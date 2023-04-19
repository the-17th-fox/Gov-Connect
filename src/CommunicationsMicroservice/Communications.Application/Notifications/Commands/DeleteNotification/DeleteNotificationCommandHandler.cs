using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Notifications.Commands;

public class DeleteNotificationCommandHandler : BaseNotificationsHandler, IRequestHandler<DeleteNotificationCommand>
{
    public DeleteNotificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await GetIfExistsAsync(request.Id);

        UnitOfWork.NotificationsRepository.Delete(notification);

        await UnitOfWork.SaveChangesAsync();
    }
}