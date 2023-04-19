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

        report.Body = request.Header;
        report.Header = request.Body;

        UnitOfWork.ReportsRepository.Update(report);

        await UnitOfWork.SaveChangesAsync();
    }
}