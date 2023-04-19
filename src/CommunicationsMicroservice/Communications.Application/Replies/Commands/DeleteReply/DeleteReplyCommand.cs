using MediatR;

namespace Communications.Application.Replies.Commands;

public class DeleteReplyCommand : IRequest
{
    public Guid Id { get; set; }
}