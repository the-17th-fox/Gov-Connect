using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class RemoveClassificationFromReportCommandHandler 
    : ReportsHandlerBase, IRequestHandler<RemoveClassificationFromReportCommand>
{
    public RemoveClassificationFromReportCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(RemoveClassificationFromReportCommand request, CancellationToken cancellationToken)
    {
        var report = await GetIfExistsAsync(request.Id);

        report.Classifications.RemoveAll(c => c.Id == request.ClassificationId);

        UnitOfWork.ReportsRepository.Update(report);

        await UnitOfWork.SaveChangesAsync();
    }
}