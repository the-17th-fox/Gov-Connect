using Microsoft.AspNetCore.Authentication;

namespace Gateway.Authentication;

public static class AuthBuilderExtensions
{
    public static AuthenticationBuilder AddCustomAuthentication(
        this AuthenticationBuilder builder, 
        string authScheme,
        Action<GovConnectAuthOptions> options)
    {
        return builder.AddScheme<GovConnectAuthOptions, GovConnectAuthHandler>(authScheme, options);
    }
}