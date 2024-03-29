using Authorities.Api.Configuration;
using Authorities.Api.Middlewares;
using SharedLib.ExceptionsHandler;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.ConfigureApplicationServices();
services.ConfigureInfrastructure(configuration);
services.ConfigureIdentity(configuration);
services.ConfigureUtilities();

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
    app.UseMiddleware<GlobalExceptionsHandler>(new[]
    {
        new ExceptionAndStatusPair(typeof(UnauthorizedAccessException), (int)HttpStatusCode.Unauthorized)
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();