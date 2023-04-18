using Microsoft.AspNetCore.Identity;

namespace Authorities.Core.Models
{
    public class User : IdentityUser<Guid>
    {
        public RefreshToken? RefreshToken { get; set; }

        public bool IsConfirmed { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
