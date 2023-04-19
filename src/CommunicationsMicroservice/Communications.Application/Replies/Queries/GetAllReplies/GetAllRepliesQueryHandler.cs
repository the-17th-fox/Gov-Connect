using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Replies.Queries;

public class GetAllRepliesQueryHandler : RepliesHandlerBase, IRequestHandler<GetAllRepliesQuery, List<Reply>>
{
    public GetAllRepliesQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<List<Reply>> Handle(GetAllRepliesQuery request, CancellationToken cancellationToken)
    {
        return await UnitOfWork.RepliesRepository.GetAllAsync(request.PageNumber, request.PageSize);
    }
}