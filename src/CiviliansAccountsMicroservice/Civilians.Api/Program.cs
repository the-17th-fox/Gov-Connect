using Civilians.Api.Configuration;
using Civilians.Api.Middlewares;

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
    app.UseMiddleware<GlobalExceptionsHandler>();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
