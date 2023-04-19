using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;

namespace Communications.Application.Classifications;

public class ClassificationsHandlerBase : HandlerBase<Classification>
{
    public ClassificationsHandlerBase(IUnitOfWork unitOfWork) : base(unitOfWork)
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