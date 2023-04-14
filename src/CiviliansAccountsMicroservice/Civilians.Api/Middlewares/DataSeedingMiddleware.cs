using Civilians.Core.Auth;
using Civilians.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Civilians.Api.Middlewares
{
    public static class DataSeedingMiddleware
    {
        public async static Task<WebApplication> SeedUsers(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService <UserManager<User>>();

            var user = await userManager.FindByEmailAsync("rootadmin@gov-connect.com");
            if(user == null)
            {
                user = CreateUser("rootadmin@gov-connect.com", "+375251250156");
                await userManager.CreateAsync(user, "RootPass1");
                await userManager.AddToRoleAsync(user, AuthRoles.Administrator);
            }

            user = await userManager.FindByEmailAsync("defaultuser@gov-connect.com");
            if(user == null)
            {
                user = CreateUser("defaultuser@gov-connect.com", "+375651052152");
                await userManager.CreateAsync(user, "DefaultPass1");
                await userManager.AddToRoleAsync(user, AuthRoles.DefaultUser);
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
                PhoneNumber = phoneNumber
            };
        }
    }
}
