using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Reports.Queries;

public class GetAllReportsByCivilianQueryHandler : ReportsHandlerBase, IRequestHandler<GetAllReportsByCivilianQuery, List<Report>>
{
    public GetAllReportsByCivilianQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<List<Report>> Handle(GetAllReportsByCivilianQuery request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.ReportsRepository.GetAllByCivilianAsync(request.CivilianId, request.PageNumber, request.PageSize);
    }
}