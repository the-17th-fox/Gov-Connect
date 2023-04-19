using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;

namespace Communications.Application.Classifications;

public class BaseClassificationsHandler : HandlerBase<Classification>
{
    public BaseClassificationsHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected override async Task<Classification> GetIfExistsAsync(Guid id)
    {
        var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(id);
        if (classification == null)
        {
            throw new NotFoundException();
        }

        return classification;
    }
}