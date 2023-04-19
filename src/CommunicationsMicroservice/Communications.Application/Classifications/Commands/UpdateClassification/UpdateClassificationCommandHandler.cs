using System.Data;
using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Commands;

public class UpdateClassificationCommandHandler : BaseClassificationsHandler, IRequestHandler<UpdateClassificationCommand>
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