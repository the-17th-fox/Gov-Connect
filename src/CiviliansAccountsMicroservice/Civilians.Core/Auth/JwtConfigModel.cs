using Microsoft.IdentityModel.Tokens;

namespace Civilians.Core.Auth
{
    public class JwtConfigModel
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AuthTokenLifetimeInMinutes { get; set; }
        public int RefreshTokenLifetimeInDays { get; set; }
        public string SecurityAlgorithm { get; set; } = string.Empty;

        public SymmetricSecurityKey? Key { get; set; }
    }
}
