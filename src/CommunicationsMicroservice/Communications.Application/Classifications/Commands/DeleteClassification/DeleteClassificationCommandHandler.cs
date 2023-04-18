using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Classifications.Commands.DeleteClassification;

public class DeleteClassificationCommandHandler : HandlerBase, IRequestHandler<DeleteClassificationCommand>
{
    public DeleteClassificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(DeleteClassificationCommand request, CancellationToken cancellationToken)
    {
        var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(request.Id);
        if (classification == null)
        {
            throw new NotFoundException();
        }

        UnitOfWork.ClassificationsRepository.Delete(classification);

        await UnitOfWork.SaveChangesAsync();
    }
}