using Microsoft.AspNetCore.Authentication;

namespace NotesApi.RefreshCookieAuthScheme;

public static class RefreshCookieExtensions
{
    public static AuthenticationBuilder AddRefreshCookie(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        Action<RefreshCookieOptions> configureOptions)
    {
        return builder.AddScheme<RefreshCookieOptions, RefreshCookieHandler>
            (authenticationScheme, configureOptions);
    }
    
    public static AuthenticationBuilder AddRefreshCookie(
        this AuthenticationBuilder builder,
        string displayName,
        string authenticationScheme,
        Action<RefreshCookieOptions> configureOptions)
    {
        return builder.AddScheme<RefreshCookieOptions, RefreshCookieHandler>
            (authenticationScheme, displayName, configureOptions);
    }
}