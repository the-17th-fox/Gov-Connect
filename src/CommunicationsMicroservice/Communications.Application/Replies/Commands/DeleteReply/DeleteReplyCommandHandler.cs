using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Replies.Commands;

public class DeleteReplyCommandHandler : RepliesHandlerBase, IRequestHandler<DeleteReplyCommand>
{
    public DeleteReplyCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(DeleteReplyCommand request, CancellationToken cancellationToken)
    {
        var reply = await GetIfExistsAsync(request.Id);

        UnitOfWork.RepliesRepository.Delete(reply);

        await UnitOfWork.SaveChangesAsync();
    }
}