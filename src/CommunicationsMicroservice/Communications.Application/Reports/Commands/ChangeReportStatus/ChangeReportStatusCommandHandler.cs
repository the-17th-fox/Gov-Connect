using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class ChangeReportStatusCommandHandler : ReportsHandlerBase, IRequestHandler<ChangeReportStatusCommand>
{
    public ChangeReportStatusCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(ChangeReportStatusCommand request, CancellationToken cancellationToken)
    {
        var report = await GetIfExistsAsync(request.Id);

        report.ReportStatus = request.ReportStatus;

        UnitOfWork.ReportsRepository.Update(report);

        await UnitOfWork.SaveChangesAsync();
    }
}