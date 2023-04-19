using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;

namespace Communications.Application.Replies.Commands;

public class UpdateReplyCommandHandler : RepliesHandlerBase, IRequestHandler<UpdateReplyCommand>
{
    public UpdateReplyCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(UpdateReplyCommand request, CancellationToken cancellationToken)
    {
        var reply = await GetIfExistsAsync(request.Id);

        if (reply.WasRead)
        {
            throw new BadRequestException("Reply had been already read, editing isn't possible.");
        }

        reply.Header = request.Header;
        reply.Body = request.Body;

        UnitOfWork.RepliesRepository.Update(reply);

        await UnitOfWork.SaveChangesAsync();
    }
}