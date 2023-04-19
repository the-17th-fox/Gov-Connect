using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Notifications.Commands;

public class AddClassificationToNotificationCommandHandler 
    : NotificationsHandlerBase, IRequestHandler<AddClassificationToNotificationCommand>
{
    public AddClassificationToNotificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(AddClassificationToNotificationCommand request, CancellationToken cancellationToken)
    {
        var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(request.Id);
        if (classification == null)
        {
            throw new NotFoundException("Classification with the specified id does not exist.");
        }

        var notification = await GetIfExistsAsync(request.Id);

        notification.Classifications.Add(classification);

        UnitOfWork.NotificationsRepository.Update(notification);

        await UnitOfWork.SaveChangesAsync();
    }
}