using Communications.Api.Configuration;
using Communications.Api.Middlewares;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.ConfigureInfrastructure(configuration);
services.ConfigureUtilities();

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

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();
