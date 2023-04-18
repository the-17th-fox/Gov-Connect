using Authorities.Core.Auth;
using Authorities.Core.Models;
using Authorities.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authorities.Api.Configuration
{
    internal static class AuthConfiguration
    {
        internal static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = true;
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AuthoritiesDbContext>();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Key"]));

            services.AddAuthentication(authOpt =>
            {
                authOpt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOpt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authOpt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOpt =>
                {
                    jwtOpt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["Authentication:Jwt:Issuer"],
                        ValidAudience = configuration["Authentication:Jwt:Audience"],
                        IssuerSigningKey = signingKey
                    };
                });

            services.Configure<JwtConfigModel>(configuration.GetSection("Authentication").GetSection("Jwt"));
            services.Configure<JwtConfigModel>(opt =>
                opt.Key = signingKey);
        }

        internal static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(AuthPolicies.DefaultRights, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(AuthRoles.Roles.Select(r => r.Name));
                });

                opt.AddPolicy(AuthPolicies.Administrators, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(
                        AuthRoles.Administrator);
                });
            });
        }
    }
}
