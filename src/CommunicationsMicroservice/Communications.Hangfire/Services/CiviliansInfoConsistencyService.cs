using AutoMapper;
using Communications.Application.AutoMapper.CiviliansInfoConsistency;
using Communications.Application.Reports.Commands;
using Communications.Hangfire.Interfaces;
using MediatR;

namespace Communications.Hangfire.Services;

public class CiviliansInfoConsistencyService : ICiviliansInfoConsistencyService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public const string RecurringJobId = "civilians-info-in-reports-update";

    public CiviliansInfoConsistencyService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task UpdateCiviliansInfoAsync(CancellationToken cancellationToken)
    {
        var civiliansDataViewModel = await GetUpdatedCiviliansDataAsync(cancellationToken);

        var updateCommand = new UpdateReportsByCiviliansCommand()
        {
            CiviliansData = civiliansDataViewModel
        };

        await _mediator.Send(updateCommand, cancellationToken);

        await Task.CompletedTask;
    }

    private async Task<List<CivilianInfoViewModel>> GetUpdatedCiviliansDataAsync(CancellationToken stoppingToken)
    {
        // Add taking new messages from kafka here,
        // possibly another lib for kafka
        throw new NotImplementedException();
    }
}