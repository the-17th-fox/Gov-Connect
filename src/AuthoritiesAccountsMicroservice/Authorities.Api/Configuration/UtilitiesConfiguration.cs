using Authorities.Api.ViewModels;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Authorities.Api.Configuration
{
    internal static class UtilitiesConfiguration
    {
        internal static void ConfigureUtilities(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApiMapperProfile));
        }

        internal static void ConfigureSwagger(this SwaggerGenOptions opt)
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthoritiesAccounts", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer ....\"",
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        }
    }
}
