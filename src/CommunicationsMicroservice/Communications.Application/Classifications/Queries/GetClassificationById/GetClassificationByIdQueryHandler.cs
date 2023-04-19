using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Queries;

public class GetClassificationByIdQueryHandler : BaseClassificationsHandler, IRequestHandler<GetClassificationByIdQuery, Classification>
{
    public GetClassificationByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Classification> Handle(GetClassificationByIdQuery request, CancellationToken cancellationToken)
    {
        var classification = await GetIfExistsAsync(request.Id);

        return classification;
    }

    
}