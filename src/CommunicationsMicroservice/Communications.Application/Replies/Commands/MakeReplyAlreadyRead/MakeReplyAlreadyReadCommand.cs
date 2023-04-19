using MediatR;

namespace Communications.Application.Replies.Commands.MakeReplyAlreadyRead;

public class MakeReplyAlreadyReadCommand : IRequest
{
    public Guid Id { get; set; }
}