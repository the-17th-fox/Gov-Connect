using Communications.Core.Misc;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class ChangeReportStatusCommand : IRequest
{
    public Guid Id { get; set; }
    public ReportStatuses ReportStatus { get; set; }
}