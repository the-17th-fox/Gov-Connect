using MediatR;

namespace Communications.Application.BaseMethods;

public class BaseSearchQuery<TEntity> : IRequest<List<TEntity>>
{
    public string Query { get; set; } = string.Empty;
}