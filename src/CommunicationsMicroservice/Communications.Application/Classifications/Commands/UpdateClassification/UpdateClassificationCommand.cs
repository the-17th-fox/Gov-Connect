using MediatR;

namespace Communications.Application.Classifications.Commands;

public class UpdateClassificationCommand : IRequest
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}