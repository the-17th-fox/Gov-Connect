using Microsoft.AspNetCore.Identity;

namespace Civilians.Core.Auth
{
    public class AuthRoles
    {
        public const string Civilian = "Civilian";
        public const string Administrator = "Administrator";

        public static List<IdentityRole<Guid>> Roles { get; } = new()
        {
            new IdentityRole<Guid>(Civilian) { Id = Guid.NewGuid(), NormalizedName = Civilian.Normalize() },
            new IdentityRole<Guid>(Administrator) { Id = Guid.NewGuid(), NormalizedName = Administrator.Normalize() }
        };
    }
}
