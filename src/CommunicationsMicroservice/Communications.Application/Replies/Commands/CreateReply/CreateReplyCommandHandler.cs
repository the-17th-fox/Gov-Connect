using AutoMapper;
using Communications.Core.Interfaces;
using Communications.Core.Misc;
using Communications.Core.Models;
using MediatR;
using SharedLib.ExceptionsHandler.CustomExceptions;

namespace Communications.Application.Replies.Commands;

public class CreateReplyCommandHandler : RepliesHandlerBase, IRequestHandler<CreateReplyCommand>
{
    private readonly IMapper _mapper;

    public CreateReplyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(CreateReplyCommand request, CancellationToken cancellationToken)
    {
        var report = await UnitOfWork.ReportsRepository.GetByIdAsync(request.ReportId);
        if (report == null)
        {
            throw new NotFoundException("Report with the specified id does not exist.");
        }

        var existingReply = await UnitOfWork.RepliesRepository.GetForReportAsync(request.ReportId);
        if (existingReply != null)
        {
            throw new AlreadyExistsException();
        }

        report.ReportStatus = ReportStatuses.Processed;

        var reply = _mapper.Map<Reply>(request);

        UnitOfWork.ReportsRepository.Update(report);
        UnitOfWork.RepliesRepository.Create(reply);

        await UnitOfWork.SaveChangesAsync();
    }
}