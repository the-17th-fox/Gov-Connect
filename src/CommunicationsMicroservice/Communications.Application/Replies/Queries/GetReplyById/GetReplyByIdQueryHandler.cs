using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Replies.Queries;

public class GetReplyByIdQueryHandler : RepliesHandlerBase, IRequestHandler<GetReplyByIdQuery, Reply>
{
    public GetReplyByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Reply> Handle(GetReplyByIdQuery request, CancellationToken cancellationToken)
    {
        return await GetIfExistsAsync(request.Id);
    }
}