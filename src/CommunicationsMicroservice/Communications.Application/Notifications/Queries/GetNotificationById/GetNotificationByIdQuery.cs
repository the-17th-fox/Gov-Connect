using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Notifications.Queries;

public class GetNotificationByIdQuery : IRequest<Notification>
{
    public Guid Id { get; set; }
}