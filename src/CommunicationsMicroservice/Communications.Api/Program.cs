using Communications.Api.Configuration;
using Communications.Api.Middlewares;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.SignalR.Hubs;
using Hangfire;
using SharedLib.ElasticSearch.Middlewares;

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
    app.UseMiddleware<GlobalExceptionsHandler>();
}

await app.CreateIndexesAsync(
    new(configuration["ElasticSearch:ReportsIndexName"], typeof(IndexedMessageViewModel)), 
    new(configuration["ElasticSearch:NotificationsIndexName"], typeof(IndexedMessageViewModel)));

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard();

app.MapHub<RepliesHub>("/repliesHub");
app.MapHub<ReportsHub>("/reportsHub");
app.MapHub<NotificationsHub>("/notificationsHub");

app.Run();
