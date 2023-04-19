using MediatR;

namespace Communications.Application.Notifications.Commands;

public class AddClassificationToNotificationCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid ClassificationId { get; set; }
}