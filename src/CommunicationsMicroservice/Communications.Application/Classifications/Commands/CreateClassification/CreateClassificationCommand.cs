using MediatR;

namespace Communications.Application.Classifications.Commands;

public class CreateClassificationCommand : IRequest
{
    public string Name { get; set; } = string.Empty;
}