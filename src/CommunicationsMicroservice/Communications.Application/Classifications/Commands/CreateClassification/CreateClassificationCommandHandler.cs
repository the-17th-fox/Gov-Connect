using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Commands;

public class CreateClassificationCommandHandler : ClassificationsHandlerBase, IRequestHandler<CreateClassificationCommand>
{
    public CreateClassificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) 
    {
    }

    public async Task Handle(CreateClassificationCommand request, CancellationToken cancellationToken)
    {
        var existingClassification = await UnitOfWork.ClassificationsRepository.GetByNameAsync(request.Name);
        if (existingClassification != null)
        {
            throw new AlreadyExistsException();
        }

        var classification = new Classification()
        {
            Name = request.Name
        };

        UnitOfWork.ClassificationsRepository.Create(classification);
        
        await UnitOfWork.SaveChangesAsync();
    }
}