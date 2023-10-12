using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;
using SharedLib.ExceptionsHandler.CustomExceptions;

namespace Communications.Application.Replies.Queries;

public class GetReplyByReportQueryHandler : RepliesHandlerBase, IRequestHandler<GetReplyByReportQuery, Reply>
{
    public GetReplyByReportQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Reply> Handle(GetReplyByReportQuery request, CancellationToken cancellationToken)
    {
        var report = await UnitOfWork.RepliesRepository.GetForReportAsync(request.ReportId);
        if (report == null)
        {
            throw new NotFoundException();
        }

        return report;
    }
}