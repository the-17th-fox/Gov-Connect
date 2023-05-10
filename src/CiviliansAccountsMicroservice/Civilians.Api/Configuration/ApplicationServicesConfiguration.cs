using Civilians.Application.Interfaces;
using Civilians.Application.Services;

namespace Civilians.Api.Configuration
{
    internal static class ApplicationServicesConfiguration
    {
        internal static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IPassportsService, PassportsService>();

            return services;
        }
    }
}
