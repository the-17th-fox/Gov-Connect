using AutoMapper;
using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

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
        var isReportExists = await UnitOfWork.ReportsRepository.CheckIfExistsAsync(request.ReportId);
        if (!isReportExists)
        {
            throw new NotFoundException("Report with the specified id does not exists.");
        }

        var existingReply = await UnitOfWork.RepliesRepository.GetForReportAsync(request.ReportId);
        if (existingReply != null)
        {
            throw new AlreadyExistsException();
        }

        var reply = _mapper.Map<Reply>(request);

        UnitOfWork.RepliesRepository.Create(reply);

        await UnitOfWork.SaveChangesAsync();
    }
}