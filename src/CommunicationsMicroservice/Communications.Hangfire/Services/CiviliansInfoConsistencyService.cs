using Communications.Application.AutoMapper.CiviliansInfoConsistency;
using Communications.Application.Reports.Commands;
using Communications.Hangfire.Interfaces;
using MediatR;
using SharedLib.Kafka.Interfaces;

namespace Communications.Hangfire.Services;

public class CiviliansInfoConsistencyService : ICiviliansInfoConsistencyService
{
    private readonly IMediator _mediator;
    private readonly IKafkaConsumer<CivilianInfoViewModel> _consumer;

    public const string RecurringJobId = "civilians-info-in-reports-update";

    public CiviliansInfoConsistencyService(IMediator mediator, IKafkaConsumer<CivilianInfoViewModel> consumer)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
    }

    public async Task UpdateCiviliansInfoAsync(CancellationToken cancellationToken)
    {
        var civiliansDataViewModel = ReadUpdatedCiviliansInfo();

        var updateCommand = new UpdateReportsByCiviliansCommand()
        {
            CiviliansData = civiliansDataViewModel
        };

        if (civiliansDataViewModel.Any())
        {
            await _mediator.Send(updateCommand, cancellationToken);
        }

        await Task.CompletedTask;
    }

    private List<CivilianInfoViewModel> ReadUpdatedCiviliansInfo()
    {
        var civiliansInfo = new List<CivilianInfoViewModel>();
        var continueReading = true;

        while (continueReading)
        {
            _consumer.Consume(out var civilianInfo);

            if (civilianInfo == null)
            {
                continueReading = false;
                continue;
            }

            civiliansInfo.Add(civilianInfo);
        }

        return civiliansInfo;
    }
}