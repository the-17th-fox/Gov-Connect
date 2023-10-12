using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Reports.Queries;

public class GetAllPendingReportsQueryHandler : ReportsHandlerBase, IRequestHandler<GetAllPendingReportsQuery, List<Report>>
{
    public GetAllPendingReportsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<List<Report>> Handle(GetAllPendingReportsQuery request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.ReportsRepository.GetAllPendingAsync(request.PageNumber, request.PageSize);
    }
}