using Civilians.Api.ViewModels;
using Civilians.Application.ViewModels;
using Microsoft.OpenApi.Models;
using SharedLib.Kafka.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Civilians.Api.Configuration
{
    internal static class UtilitiesConfiguration
    {
        internal static IServiceCollection ConfigureUtilities(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAutoMapper(typeof(ApplicationMapperProfile), typeof(ApiMapperProfile));
            
            services.ConfigureProducers(configuration.GetConnectionString("KafkaBootstrapServers"));

            return services;
        }

        internal static void ConfigureSwagger(this SwaggerGenOptions opt)
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CiviliansAccounts", Version = "v1" });
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

        private static IServiceCollection ConfigureProducers(this IServiceCollection services, string bootstrapServers)
        {
            services.AddKafkaProducer<string, object>(opt =>
            {
                opt.Topic = "civilians-info-in-reports-update";
                opt.BootstrapServers = bootstrapServers;
            });

            return services;
        }
    }
}
