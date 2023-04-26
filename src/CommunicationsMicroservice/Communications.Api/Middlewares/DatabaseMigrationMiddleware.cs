using Communications.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Communications.Api.Middlewares;

public static class DatabaseMigrationMiddleware
{
    public static async Task<WebApplication> MigrateDatabase(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<CommunicationsDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
        }

        return app;
    }
}