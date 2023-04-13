using Microsoft.AspNetCore.Identity;

namespace Civilians.Core.Models
{
    public class User : IdentityUser<Guid>
    {
        public Passport Passport { get; set; } = null!;

        public RefreshToken? RefreshToken { get; set; }

        public bool IsBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
