using Microsoft.AspNetCore.Identity;

namespace Civilians.Core.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid? PassportdId { get; set; }
        public Passport? Passport { get; set; }

        public Guid? RefreshTokenId { get; set; }
        public RefreshToken? RefreshToken { get; set; }

        public bool IsDeleted { get; set; }
    }
}
