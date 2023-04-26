using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class DeleteReportCommandHandler : ReportsHandlerBase, IRequestHandler<DeleteReportCommand>
{
    public DeleteReportCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        var report = await GetIfExistsAsync(request.Id);

        if (!report.CanBeEdited)
        {
            throw new BadRequestException("Report can not be edited anymore.");
        }

        UnitOfWork.ReportsRepository.Delete(report);

        await UnitOfWork.SaveChangesAsync();
    }
}