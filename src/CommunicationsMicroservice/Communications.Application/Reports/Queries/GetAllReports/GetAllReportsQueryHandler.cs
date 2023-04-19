using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Reports.Queries;

public class GetAllReportsQueryHandler : ReportsHandlerBase, IRequestHandler<GetAllReportsQuery, List<Report>>
{
    public GetAllReportsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<List<Report>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.ReportsRepository.GetAllAsync(request.PageNumber, request.PageSize);
    }
}