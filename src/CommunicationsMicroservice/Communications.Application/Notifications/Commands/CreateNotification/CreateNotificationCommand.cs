using MediatR;

namespace Communications.Application.Notifications.Commands;

public class CreateNotificationCommand : IRequest
{
    public Guid AuthorityId { get; set; }
    public string Organization { get; set; } = string.Empty;
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<Guid> ClassificationsIds { get; set; } = new();
}