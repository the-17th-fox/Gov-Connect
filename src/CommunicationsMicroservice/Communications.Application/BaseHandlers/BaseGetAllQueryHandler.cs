using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.BaseHandlers;

public class BaseGetAllQueryHandler<TRequest, TEntity, TRepository> : IRequestHandler<TRequest, List<TEntity>>
    where TRequest : IRequest<List<TEntity>>
    where TEntity : class
    where TRepository : IGenericRepository<TEntity>
{
    protected readonly TRepository Repository;

    public BaseGetAllQueryHandler(TRepository repository)
    {
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<List<TEntity>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var response = await Repository.GetAllAsync(request.PageNumber, request.PageSize);
    }
}