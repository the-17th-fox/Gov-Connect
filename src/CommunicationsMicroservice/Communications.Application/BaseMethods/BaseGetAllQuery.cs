using MediatR;

namespace Communications.Application.BaseMethods;

public abstract class BaseGetAllQuery<TEntity> : IRequest<List<TEntity>>
{
    public short PageNumber { get; set; }
    public byte PageSize { get; set; }
}