using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Commands;

public class CreateClassificationCommandHandler : HandlerBase, IRequestHandler<CreateClassificationCommand>
{
    public CreateClassificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) 
    {
    }

    public async Task Handle(CreateClassificationCommand request, CancellationToken cancellationToken)
    {
        var classification = new Classification()
        {
            Name = request.Name
        };

        UnitOfWork.ClassificationsRepository.Create(classification);
        
        await UnitOfWork.SaveChangesAsync();
    }
}