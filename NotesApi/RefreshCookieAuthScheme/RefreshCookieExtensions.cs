using Microsoft.AspNetCore.Authentication;

namespace NotesApi.RefreshCookieAuthScheme;

public static class RefreshCookieExtensions
{
    public static AuthenticationBuilder AddRefreshCookie(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        string displayName,
        Action<RefreshCookieOptions> configureOptions)
    {
        return builder.AddScheme<RefreshCookieOptions, RefreshCookieHandler>
            (authenticationScheme, displayName, configureOptions);
    }
}