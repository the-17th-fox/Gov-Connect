using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class AddClassificationToReportCommandHandler
    : ReportsHandlerBase, IRequestHandler<AddClassificationToReportCommand>
{
    public AddClassificationToReportCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(AddClassificationToReportCommand request, CancellationToken cancellationToken)
    {
        var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(request.ClassificationId);
        if (classification == null)
        {
            throw new NotFoundException("Classification with the specified id doesn't exist.");
        }

        var report = await GetIfExistsAsync(request.Id);

        report.Classifications.Add(classification);

        UnitOfWork.ReportsRepository.Update(report);

        await UnitOfWork.SaveChangesAsync();
    }
}