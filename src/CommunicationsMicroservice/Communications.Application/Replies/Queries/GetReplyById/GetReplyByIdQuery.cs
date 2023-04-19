using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Replies.Queries;

public class GetReplyByIdQuery : IRequest<Reply>
{
    public Guid Id { get; set; }
    public Guid CivilianId { get; set; }
}