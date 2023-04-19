using MediatR;

namespace Communications.Application.Reports.Commands;

public class AddClassificationToReportCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid ClassificationId { get; set; }
}