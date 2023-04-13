using Civilians.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Api.Middlewares
{
    public static class DatabaseMigrationMiddleware
    {
        public async static Task<WebApplication> MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CiviliansDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            return app;
        }
    }
}
