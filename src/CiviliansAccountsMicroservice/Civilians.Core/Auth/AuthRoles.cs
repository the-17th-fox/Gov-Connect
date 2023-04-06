using Microsoft.AspNetCore.Identity;

namespace Civilians.Core.Auth
{
    public class AuthRoles
    {
        public const string DefaultUser = "DefaultUser";
        public const string Administrator = "Administrator";

        public static List<IdentityRole<Guid>> Roles { get; } = new()
        {
            new IdentityRole<Guid>(DefaultUser) { Id = Guid.NewGuid(), NormalizedName = DefaultUser.Normalize() },
            new IdentityRole<Guid>(Administrator) { Id = Guid.NewGuid(), NormalizedName = Administrator.Normalize() }
        };
    }
}
