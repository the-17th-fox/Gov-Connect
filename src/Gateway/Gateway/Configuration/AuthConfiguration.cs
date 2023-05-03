using Gateway.Authentication;

namespace Gateway.Configuration;

public static class AuthConfiguration
{
    public static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
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
    }
}