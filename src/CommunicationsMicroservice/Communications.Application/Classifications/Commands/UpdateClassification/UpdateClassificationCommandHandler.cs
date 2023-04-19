using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Classifications.Commands;

public class UpdateClassificationCommandHandler : ClassificationsHandlerBase, IRequestHandler<UpdateClassificationCommand>
{
    public UpdateClassificationCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(UpdateClassificationCommand request, CancellationToken cancellationToken)
    {
        var classification = await GetIfExistsAsync(request.Id);

        classification.Name = request.NewName;

        UnitOfWork.ClassificationsRepository.Update(classification);

        await UnitOfWork.SaveChangesAsync();
    }
}