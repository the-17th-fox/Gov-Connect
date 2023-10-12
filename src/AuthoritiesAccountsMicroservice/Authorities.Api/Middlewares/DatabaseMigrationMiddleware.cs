using Authorities.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Authorities.Api.Middlewares
{
    public static class DatabaseMigrationMiddleware
    {
        public static async Task<WebApplication> MigrateDatabase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            await using var context = scope.ServiceProvider.GetRequiredService<AuthoritiesDbContext>();

            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.EnsureCreatedAsync();
                await context.Database.MigrateAsync();
            }

            return app;
        }
    }
}
