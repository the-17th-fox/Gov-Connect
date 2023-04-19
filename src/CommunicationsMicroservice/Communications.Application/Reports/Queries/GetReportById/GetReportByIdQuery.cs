using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Reports.Queries.GetReportById;

public class GetReportByIdQuery : IRequest<Report>
{
    public Guid Id { get; set; }
}