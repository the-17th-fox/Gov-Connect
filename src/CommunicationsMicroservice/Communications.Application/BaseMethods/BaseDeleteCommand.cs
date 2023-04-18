using MediatR;

namespace Communications.Application.BaseMethods;

public class BaseDeleteCommand : IRequest
{
    public Guid Id { get; set; }
}