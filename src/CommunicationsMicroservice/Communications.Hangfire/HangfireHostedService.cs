using Communications.Hangfire.Interfaces;
using Communications.Hangfire.Services;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace Communications.Hangfire;

public class HangfireHostedService : BackgroundService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ICiviliansInfoConsistencyService _civiliansInfoConsistencyService;

    public HangfireHostedService(IRecurringJobManager recurringJobManager, ICiviliansInfoConsistencyService civiliansInfoConsistencyService)
    {
        _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));

        _civiliansInfoConsistencyService = civiliansInfoConsistencyService ??
                                           throw new ArgumentNullException(nameof(civiliansInfoConsistencyService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _recurringJobManager.AddOrUpdate(
            CiviliansInfoConsistencyService.RecurringJobId,
            () => _civiliansInfoConsistencyService.UpdateCiviliansInfoAsync(stoppingToken),
            Cron.Hourly());

        await Task.CompletedTask;
    }
}