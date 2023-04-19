using Authorities.Core.Interfaces;
using Authorities.Infrastructure.DbContext;
using Authorities.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authorities.Api.Configuration
{
    internal static class InfrastructureConfiguration
    {
        internal static void ConfigureInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("DatabaseConnection");

            services.AddDbContext<AuthoritiesDbContext>(opt =>
                opt.UseSqlServer(dbConnectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
