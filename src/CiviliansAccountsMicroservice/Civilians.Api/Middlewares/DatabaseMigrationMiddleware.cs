using Civilians.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Api.Middlewares
{
    public static class DatabaseMigrationMiddleware
    {
        public static async Task<WebApplication> MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CiviliansDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.EnsureCreatedAsync();
                await context.Database.MigrateAsync();
            }

            return app;
        }
    }
}
