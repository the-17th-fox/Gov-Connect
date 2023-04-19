using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application;

public abstract class HandlerBase<TEntity>
{
    protected readonly IUnitOfWork UnitOfWork;

    protected HandlerBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Trying to get the entity from the repository.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Entity, otherwise throws NotFoundException</returns>
    protected abstract Task<TEntity> GetIfExistsAsync(Guid id);
}