using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class UpdateReportsByCiviliansCommandHandler : ReportsHandlerBase, IRequestHandler<UpdateReportsByCiviliansCommand>
{
    public UpdateReportsByCiviliansCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(UpdateReportsByCiviliansCommand request, CancellationToken cancellationToken)
    {
        var civiliansIds = request.CiviliansData.Select(cd => cd.CivilianId);
        var reports = await UnitOfWork.ReportsRepository.GetAllByCiviliansAsync(civiliansIds);

        foreach (var report in reports)
        {
            var civilianData = request.CiviliansData.FirstOrDefault(cd => cd.CivilianId == report.CivilianId);

            if (civilianData != null)
            {
                UnitOfWork.ReportsRepository.Update(report);
                report.FirstName = civilianData.FirstName;
                report.Patronymic = civilianData.Patronymic;
            }
        }

        await UnitOfWork.SaveChangesAsync();
    }
}