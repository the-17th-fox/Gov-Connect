using MediatR;

namespace Communications.Application.Replies.Commands;

public class UpdateReplyCommand : IRequest
{
    public Guid Id { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}