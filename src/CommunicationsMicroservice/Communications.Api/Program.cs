using System.Net;
using Communications.Api.Configuration;
using Communications.Api.Middlewares;
using Communications.SignalR.Hubs;
using Hangfire;
using SharedLib.ExceptionsHandler;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .ConfigureInfrastructure(configuration)
    .ConfigureServices()
    .ConfigureUtilities(configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabase();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseMiddleware<GlobalExceptionsHandler>(new[]
    {
        new ExceptionAndStatusPair(typeof(UnauthorizedAccessException), (int)HttpStatusCode.Unauthorized)
    });
}

await app.ConfigureElasticIndexes(configuration, elasticSearchSectionPath: "ElasticSearchConfiguration");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard();

app.MapHub<RepliesHub>("/repliesHub");
app.MapHub<ReportsHub>("/reportsHub");
app.MapHub<NotificationsHub>("/notificationsHub");

app.Run();
