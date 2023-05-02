using Communications.Hangfire;
using Communications.Hangfire.Interfaces;
using Communications.Hangfire.Services;

namespace Communications.Api.Configuration;

public static class ServicesConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<ICiviliansInfoConsistencyService, CiviliansInfoConsistencyService>();

        services.AddHostedService<HangfireHostedService>();
    }
}