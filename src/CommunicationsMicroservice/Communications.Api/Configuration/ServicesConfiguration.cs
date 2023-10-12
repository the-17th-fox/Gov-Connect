using Communications.Hangfire;
using Communications.Hangfire.Interfaces;
using Communications.Hangfire.Services;

namespace Communications.Api.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<ICiviliansInfoConsistencyService, CiviliansInfoConsistencyService>();

        services.AddHostedService<HangfireHostedService>();

        return services;
    }
}