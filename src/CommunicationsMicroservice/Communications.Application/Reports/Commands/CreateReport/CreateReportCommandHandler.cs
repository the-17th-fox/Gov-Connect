using AutoMapper;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class CreateReportCommandHandler : ReportsHandlerBase, IRequestHandler<CreateReportCommand>
{
    private readonly IMapper _mapper;

    public CreateReportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var report = _mapper.Map<Report>(request);

        foreach (var classificationId in request.ClassificationsIds)
        {
            var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(classificationId);
            if (classification != null)
            {
                report.Classifications.Add(classification);
            }
        }

        UnitOfWork.ReportsRepository.Create(report);

        await UnitOfWork.SaveChangesAsync();
    }
}