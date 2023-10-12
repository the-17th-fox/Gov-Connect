using MediatR;

namespace Communications.Application.Reports.Commands;

public class UpdateReportCommand : IRequest
{
    public Guid Id { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<Guid> ClassificationsIds { get; set; } = new();
}