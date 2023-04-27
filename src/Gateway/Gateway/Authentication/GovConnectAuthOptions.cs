using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Authentication;

public class GovConnectAuthOptions : AuthenticationSchemeOptions
{
    public const string Scheme = "GovConnectAuthentication";
    public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();
}