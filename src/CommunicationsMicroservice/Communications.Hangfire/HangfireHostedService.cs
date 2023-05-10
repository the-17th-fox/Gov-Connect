using Communications.Hangfire.Interfaces;
using Communications.Hangfire.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Communications.Hangfire;

public class HangfireHostedService : BackgroundService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IServiceScopeFactory _scopeFactory;

    public HangfireHostedService(IRecurringJobManager recurringJobManager, IServiceScopeFactory scopeFactory)
    {
        _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var civiliansInfoConsistencyService = scope.ServiceProvider.GetRequiredService<ICiviliansInfoConsistencyService>();

        _recurringJobManager.AddOrUpdate(
            CiviliansInfoConsistencyService.RecurringJobId,
            () => civiliansInfoConsistencyService.UpdateCiviliansInfoAsync(stoppingToken),
            Cron.Hourly);

        await Task.CompletedTask;
    }
}