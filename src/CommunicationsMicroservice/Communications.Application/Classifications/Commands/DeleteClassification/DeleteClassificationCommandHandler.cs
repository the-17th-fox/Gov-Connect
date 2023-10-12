using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Classifications.Commands.DeleteClassification;

public class DeleteClassificationCommandHandler : ClassificationsHandlerBase, IRequestHandler<DeleteClassificationCommand>
{
    public DeleteClassificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(DeleteClassificationCommand request, CancellationToken cancellationToken)
    {
        var classification = await GetIfExistsAsync(request.Id);

        UnitOfWork.ClassificationsRepository.Delete(classification);

        await UnitOfWork.SaveChangesAsync();
    }
}