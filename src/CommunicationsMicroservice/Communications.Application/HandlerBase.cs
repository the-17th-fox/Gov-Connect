using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application;

public abstract class HandlerBase
{
    protected readonly IUnitOfWork UnitOfWork;

    protected HandlerBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
}