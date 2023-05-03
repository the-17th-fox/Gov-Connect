using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Infrastructure.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Gateway.Authentication;

public class GovConnectAuthHandler : AuthenticationHandler<GovConnectAuthOptions>
{
    public GovConnectAuthHandler(
        IOptionsMonitor<GovConnectAuthOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder, 
        ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
    }

    private const string _authorizationHeaderName = "Authorization";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var accessToken = Request.Headers[_authorizationHeaderName].GetValue();

        if (string.IsNullOrEmpty(accessToken))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!accessToken.Contains("Bearer "))
        {
            return Task.FromResult(AuthenticateResult.Fail("Token lacks 'Bearer' prefix."));
        }

        accessToken = accessToken.Remove(startIndex: 0, count: 7); // Removes 'Bearer ' from access token

        ClaimsPrincipal? principal;
        try
        {
            principal = GetTokenPrincipal(accessToken);
            if (principal == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Couldn't get token's principal."));
            }
        }
        catch (Exception e)
        {
            return Task.FromResult(AuthenticateResult.Fail(e.Message));
        }

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, GovConnectAuthOptions.Scheme)));
    }

    private ClaimsPrincipal? GetTokenPrincipal(string accessToken)
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        if (jwtSecurityToken == null)
        {
            throw new ArgumentNullException(nameof(jwtSecurityToken));
        }

        var accountsServiceName = GetAccountsServiceName(jwtSecurityToken);

        var signingSecurityKey = GetSecurityKey(accountsServiceName);

        var tokenValidationParameters = GetValidationParameters(jwtSecurityToken, signingSecurityKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out _);

        return principal;
    }

    private string GetAccountsServiceName(JwtSecurityToken jwtSecurityToken)
    {
        var accountsServiceName = jwtSecurityToken.Claims?
            .FirstOrDefault(c => c.Type == "identifyas")?.Value;

        if (string.IsNullOrEmpty(accountsServiceName))
        {
            throw new ArgumentNullException(nameof(accountsServiceName));
        }

        var servicesSection = Options.ConfigurationSection.GetChildren();

        bool isNameExists = servicesSection.Any(s =>
            s.Key.Equals(accountsServiceName, StringComparison.InvariantCultureIgnoreCase));

        if (!isNameExists)
        {
            throw new SecurityTokenException("Accounts service is invalid.");
        }

        return accountsServiceName;
    }

    private SymmetricSecurityKey GetSecurityKey(string accountsServiceName)
    {
        var decodedSigningKey = Options.ConfigurationSection.GetSection($"{accountsServiceName}").GetValue<string>("Key");

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(decodedSigningKey));
    }

    private TokenValidationParameters GetValidationParameters(JwtSecurityToken jwtSecurityToken, SecurityKey signingSecurityKey)
    {
        var tokenValidationParameters = Options.TokenValidationParameters;
        tokenValidationParameters.ValidIssuer = jwtSecurityToken.Issuer;
        tokenValidationParameters.ValidAudience = jwtSecurityToken.Audiences.FirstOrDefault();
        tokenValidationParameters.IssuerSigningKey = signingSecurityKey;

        return tokenValidationParameters;
    }
}
