using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Queries;

public class GetAllClassificationsQueryHandler : ClassificationsHandlerBase, IRequestHandler<GetAllClassificationsQuery, List<Classification>>
{
    public GetAllClassificationsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<List<Classification>> Handle(GetAllClassificationsQuery request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.ClassificationsRepository.GetAllAsync(request.PageNumber, request.PageSize);
    }
}