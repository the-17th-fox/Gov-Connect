using Gateway.Authentication;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
configuration.AddJsonFile("docker-configuration.json", optional: true, reloadOnChange: true);
services.AddOcelot(configuration);

services.AddAuthentication(options =>
{
    options.DefaultScheme = GovConnectAuthOptions.Scheme;
})
    .AddCustomAuthentication(GovConnectAuthOptions.Scheme, opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        opt.ConfigurationSection = configuration.GetSection("Authentication").GetSection("Jwt");
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseOcelot().Wait();

app.UseAuthentication();

app.Run();