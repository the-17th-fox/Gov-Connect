using MediatR;

namespace Communications.Application.Reports.Commands;

public class CreateReportCommand : IRequest
{
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid CivilianId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public List<Guid> ClassificationsIds { get; set; } = new();
}