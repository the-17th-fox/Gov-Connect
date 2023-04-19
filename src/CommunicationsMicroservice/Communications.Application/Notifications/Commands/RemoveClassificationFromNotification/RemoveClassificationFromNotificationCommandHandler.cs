using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Notifications.Commands.RemoveClassificationFromNotification;

public class RemoveClassificationFromNotificationCommandHandler 
    : NotificationsHandlerBase, IRequestHandler<RemoveClassificationFromNotificationCommand>
{
    public RemoveClassificationFromNotificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(RemoveClassificationFromNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await GetIfExistsAsync(request.Id);

        notification.Classifications.RemoveAll(c => c.Id == request.ClassificationId);

        UnitOfWork.NotificationsRepository.Update(notification);

        await UnitOfWork.SaveChangesAsync();
    }
}