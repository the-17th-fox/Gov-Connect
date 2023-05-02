using Communications.Core.Interfaces;
using Communications.Infrastructure.DbContext;
using Communications.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Communications.Api.Configuration;

internal static class InfrastructureConfiguration
{
    internal static void ConfigureInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("DatabaseConnection");

        services.AddDbContext<CommunicationsDbContext>(opt =>
            opt.UseSqlServer(dbConnectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}