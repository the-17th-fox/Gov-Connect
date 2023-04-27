using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        ISystemClock clock, 
        ConfigurationManager configuration) 
        : base(options, logger, encoder, clock)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private const string _authorizationHeaderName = "Authorization";

    private readonly IOptionsMonitor<GovConnectAuthOptions> _options;
    private readonly ConfigurationManager _configuration;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var accessToken = Request.Headers[_authorizationHeaderName];
        if (string.IsNullOrEmpty(accessToken))
        {
            return Task.FromResult(AuthenticateResult.Fail("Can not identify accounts service."));
        }

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

    private SymmetricSecurityKey GetSecurityKey(string accountsServiceName)
    {
        var decodedSigningKey = _configuration[$"Authentication:Jwt:{accountsServiceName}:Key"];

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(decodedSigningKey));
    }

    private string GetAccountsServiceName(JwtSecurityToken jwtSecurityToken)
    {
        var accountsServiceName = jwtSecurityToken.Claims?
            .FirstOrDefault(c => c.Type == "identifyas")?.Value;

        if (string.IsNullOrEmpty(accountsServiceName))
        {
            throw new ArgumentNullException(nameof(accountsServiceName));
        }

        var servicesSection = _configuration.GetSection("Authentication").GetSection("Jwt").GetChildren();

        bool isNameExists = servicesSection.Any(s =>
            s.Key.Equals(accountsServiceName, StringComparison.InvariantCultureIgnoreCase));

        if (!isNameExists)
        {
            throw new SecurityTokenException("Accounts service is invalid.");
        }

        return accountsServiceName;
    }

    private TokenValidationParameters GetValidationParameters(JwtSecurityToken jwtSecurityToken, SecurityKey signingSecurityKey)
    {
        var tokenValidationParameters = _options.CurrentValue.TokenValidationParameters;
        tokenValidationParameters.ValidIssuer = jwtSecurityToken.Issuer;
        tokenValidationParameters.ValidAudience = jwtSecurityToken.Audiences.FirstOrDefault();
        tokenValidationParameters.IssuerSigningKey = signingSecurityKey;

        return tokenValidationParameters;
    }
}