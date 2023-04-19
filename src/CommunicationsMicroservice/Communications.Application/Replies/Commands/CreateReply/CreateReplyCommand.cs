using MediatR;

namespace Communications.Application.Replies.Commands;

public class CreateReplyCommand : IRequest
{
    public Guid AuthorityId { get; set; }
    public Guid ReportId { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}