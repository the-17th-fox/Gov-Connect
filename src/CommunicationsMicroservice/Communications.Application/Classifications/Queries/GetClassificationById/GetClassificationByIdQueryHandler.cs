using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Queries;

public class GetClassificationByIdQueryHandler : ClassificationsHandlerBase, IRequestHandler<GetClassificationByIdQuery, Classification>
{
    public GetClassificationByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Classification> Handle(GetClassificationByIdQuery request, CancellationToken cancellationToken)
    {
        return await GetIfExistsAsync(request.Id);
    }

    
}