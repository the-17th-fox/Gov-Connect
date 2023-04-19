using Authorities.Core.Auth;
using Authorities.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Authorities.Api.Middlewares
{
    public static class DataSeedingMiddleware
    {
        public static async Task<WebApplication> SeedUsers(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService <UserManager<User>>();

            var user = await userManager.FindByEmailAsync("rootadmin@authorities.gov-connect.com");
            if(user != null)
            {
                user = CreateUser("rootadmin@authorities.gov-connect.com", "+375753066876");
                await userManager.CreateAsync(user, "UserPass1");
                await userManager.AddToRoleAsync(user, AuthRoles.Administrator);
            }

            user = await userManager.FindByEmailAsync("policeguy@gov-connect.com");
            if(user != null)
            {
                user = CreateUser("policeguy@gov-connect.com", "+375440099123");
                await userManager.CreateAsync(user, "UserPass1");
                await userManager.AddToRoleAsync(user, AuthRoles.MIA);
            }

            return app;
        }

        private static User CreateUser(string email, string phoneNumber)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                PhoneNumber = phoneNumber,
                IsConfirmed = true
            };
        }
    }
}
