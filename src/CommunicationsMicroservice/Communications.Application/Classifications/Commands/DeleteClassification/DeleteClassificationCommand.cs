using MediatR;

namespace Communications.Application.Classifications.Commands.DeleteClassification;

public class DeleteClassificationCommand : IRequest
{
    public Guid Id { get; set; }
}