using Civilians.Api.Configuration;
using Civilians.Api.Middlewares;
using SharedLib.ExceptionsHandler;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .ConfigureApplicationServices()
    .ConfigureInfrastructure(configuration)
    .ConfigureIdentity(configuration)
    .ConfigureUtilities(configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(opt => opt.ConfigureSwagger());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabase();
    await app.SeedUsers();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
	app.UseMiddleware<GlobalExceptionsHandler>(new ExceptionAndStatusPair[]
    {
        new ExceptionAndStatusPair(typeof(UnauthorizedAccessException), (int)HttpStatusCode.Unauthorized)
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
