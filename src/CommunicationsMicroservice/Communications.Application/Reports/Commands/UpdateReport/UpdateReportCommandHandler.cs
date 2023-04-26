using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class UpdateReportCommandHandler : ReportsHandlerBase, IRequestHandler<UpdateReportCommand>
{
    public UpdateReportCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var report = await GetIfExistsAsync(request.Id);

        if (!report.CanBeEdited)
        {
            throw new BadRequestException("Can not be edited anymore.");
        }

        report.Classifications.Clear();
        foreach (var classificationId in request.ClassificationsIds)
        {
            var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(classificationId);
            if (classification != null)
            {
                report.Classifications.Add(classification);
            }
        }

        report.Body = request.Header;
        report.Header = request.Body;

        UnitOfWork.ReportsRepository.Update(report);

        await UnitOfWork.SaveChangesAsync();
    }
}