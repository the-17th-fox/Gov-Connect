using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Notifications.Commands;

public class CreateNotificationCommandHandler : BaseNotificationsHandler, IRequestHandler<CreateNotificationCommand>
{
    public CreateNotificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification()
        {
            AuthorityId = request.AuthorityId,
            Header = request.Header,
            Body = request.Body,
            Organization = request.Organization,
        };

        UnitOfWork.NotificationsRepository.Create(notification);

        await UnitOfWork.SaveChangesAsync();
    }
}