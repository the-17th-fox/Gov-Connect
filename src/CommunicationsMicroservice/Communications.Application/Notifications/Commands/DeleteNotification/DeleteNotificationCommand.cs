using MediatR;

namespace Communications.Application.Notifications.Commands;

public class DeleteNotificationCommand : IRequest
{
    public Guid Id { get; set; }
}