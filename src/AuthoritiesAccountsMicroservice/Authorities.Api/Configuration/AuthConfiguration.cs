﻿using Authorities.Core.Auth;
using Authorities.Core.Models;
using Authorities.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authorities.Api.Configuration
{
    internal static class AuthConfiguration
    {
        internal static void ConfigureIdentity(this IServiceCollection services, ConfigurationManager configuration)
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

            services.Configure<JwtConfigModel>(configuration.GetSection("Authentication").GetSection("Jwt"));
            services.Configure<JwtConfigModel>(opt =>
                opt.Key = signingKey);
        }
    }
}
