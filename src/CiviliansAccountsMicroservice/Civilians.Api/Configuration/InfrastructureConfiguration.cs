using Civilians.Core.Interfaces;
using Civilians.Infrastructure.DbContext;
using Civilians.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Api.Configuration
{
    internal static class InfrastructureConfiguration
    {
        internal static void ConfigureInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("DatabaseConnection");

            services.AddDbContext<CiviliansDbContext>(opt =>
                opt.UseSqlServer(dbConnectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
