using Gateway.Configuration;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.ConfigureAuthentication(configuration);
services.ConfigureOcelot(configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseOcelot().Wait();

app.UseAuthentication();

app.Run();