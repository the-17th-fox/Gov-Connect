using Authorities.Application.Interfaces;
using Authorities.Application.Services;

namespace Authorities.Api.Configuration
{
    internal static class ApplicationServicesConfiguration
    {
        internal static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IRolesService, RolesService>();
        }
    }
}
