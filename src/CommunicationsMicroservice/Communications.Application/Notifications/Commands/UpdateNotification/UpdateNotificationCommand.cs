using MediatR;

namespace Communications.Application.Notifications.Commands;

public class UpdateNotificationCommand : IRequest
{
    public Guid Id { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}