using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Reports.Queries.GetReportById;

public class GetReportByIdQueryHandler : ReportsHandlerBase, IRequestHandler<GetReportByIdQuery, Report>
{
    public GetReportByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Report> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        return await GetIfExistsAsync(request.Id);
    }
}