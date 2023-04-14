using Civilians.Api.Configuration;
using Civilians.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.ConfigureApplicationServices();
services.ConfigureInfrastructure(configuration);
services.ConfigureAuthentication(configuration);
services.ConfigureAuthorization();
services.ConfigureUtilities();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(opt => opt.ConfigureSwagger());

var app = builder.Build();

app.UseMiddleware<GlobalExceptionsHandler>();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabase();
    await app.SeedUsers();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
