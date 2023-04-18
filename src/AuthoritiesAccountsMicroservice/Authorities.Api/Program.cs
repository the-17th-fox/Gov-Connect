using Authorities.Api.Configuration;
using Authorities.Api.Middlewares;

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
