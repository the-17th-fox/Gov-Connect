using MediatR;

namespace Communications.Application.Reports.Commands;

public class DeleteReportCommand : IRequest
{
    public Guid Id { get; set; }
}