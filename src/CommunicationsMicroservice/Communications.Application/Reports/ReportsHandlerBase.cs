using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;

namespace Communications.Application.Reports;

public class ReportsHandlerBase : HandlerBase<Report>
{
    public ReportsHandlerBase(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected override async Task<Report> GetIfExistsAsync(Guid id)
    {
        var report = await UnitOfWork.ReportsRepository.GetByIdAsync(id);
        if (report == null)
        {
            throw new NotFoundException();
        }

        return report;
    }
}