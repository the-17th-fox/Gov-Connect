using Communications.Core.Models;
using MediatR;

namespace Communications.Application.BaseMethods;

public class BaseGetByIdQuery<TEntity> : IRequest<TEntity>
{
    public Guid Id { get; set; }
}