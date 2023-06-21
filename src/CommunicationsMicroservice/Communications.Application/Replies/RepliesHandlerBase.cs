using Communications.Core.Interfaces;
using Communications.Core.Models;
using SharedLib.ExceptionsHandler.CustomExceptions;

namespace Communications.Application.Replies;

public class RepliesHandlerBase : HandlerBase<Reply>
{
    public RepliesHandlerBase(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected override async Task<Reply> GetIfExistsAsync(Guid id)
    {
        var reply = await UnitOfWork.RepliesRepository.GetByIdAsync(id);
        if (reply == null)
        {
            throw new NotFoundException();
        }

        return reply;
    }
}