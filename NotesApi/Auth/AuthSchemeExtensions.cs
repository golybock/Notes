using Microsoft.AspNetCore.Authentication;

namespace NotesApi.Auth;

public static class AuthSchemeExtensions
{
    public static AuthenticationBuilder AddNoOpAuth(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<AuthSchemeOptions> configureOptions)
    {
        return builder.AddScheme<AuthSchemeOptions, AuthHandler>(authenticationScheme, displayName, configureOptions);
    }
}