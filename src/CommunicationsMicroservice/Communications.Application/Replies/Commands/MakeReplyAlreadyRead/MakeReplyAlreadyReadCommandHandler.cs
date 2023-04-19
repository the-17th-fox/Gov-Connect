using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Replies.Commands.MakeReplyAlreadyRead;

public class MakeReplyAlreadyReadCommandHandler : RepliesHandlerBase, IRequestHandler<MakeReplyAlreadyReadCommand>
{
    public MakeReplyAlreadyReadCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(MakeReplyAlreadyReadCommand request, CancellationToken cancellationToken)
    {
        var reply = await GetIfExistsAsync(request.Id);

        reply.WasRead = true;

        UnitOfWork.RepliesRepository.Update(reply);

        await UnitOfWork.SaveChangesAsync();
    }
}