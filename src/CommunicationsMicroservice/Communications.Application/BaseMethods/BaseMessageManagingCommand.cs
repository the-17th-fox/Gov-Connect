using MediatR;

namespace Communications.Application.BaseMethods;

public class BaseMessageCreateCommand : IRequest
{
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}