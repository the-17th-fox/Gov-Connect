using MediatR;

namespace Communications.Application.Notifications.Commands.RemoveClassificationFromNotification;

public class RemoveClassificationFromNotificationCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid ClassificationId { get; set; }
}