using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Notifications.Commands;

public class UpdateNotificationCommandHandler : NotificationsHandlerBase, IRequestHandler<UpdateNotificationCommand>
{
    public UpdateNotificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
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
    }
}