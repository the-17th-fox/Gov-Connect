using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Queries;

public class GetClassificationByIdQuery : HandlerBase, IRequestHandler<GetClassificationByIdQuery, Classification>
{
    public GetClassificationByIdQuery(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Classification> Handle(GetClassificationByIdQuery request, CancellationToken cancellationToken)
    {
        var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(request.Id);

        if (classification == null)
        {
            throw new NotFoundException();
        }

        return classification;
    }

    
}