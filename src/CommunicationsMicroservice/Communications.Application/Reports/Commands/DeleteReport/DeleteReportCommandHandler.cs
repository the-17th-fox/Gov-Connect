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

        UnitOfWork.ReportsRepository.Delete(report);

        await UnitOfWork.SaveChangesAsync();
    }
}