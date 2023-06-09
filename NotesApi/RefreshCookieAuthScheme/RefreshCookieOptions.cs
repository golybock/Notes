﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace NotesApi.RefreshCookieAuthScheme;

public class RefreshCookieOptions : AuthenticationSchemeOptions
{
    // validate
    public bool ValidateAudience { get; set; }

    public bool ValidateIssuer { get; set; }

    public bool ValidateLifetime { get; set; }

    public bool ValidateIssuerSigningKey { get; set; }

    #region validate params

    // lifetime validate params
    public int RefreshTokenLifeTimeInDays { get; set; }

    public int TokenLifeTimeInMinutes { get; set; }

    // params
    public string? ValidAudience { get; set; }

    public string? ValidIssuer { get; set; }

    public string? Secret { get; set; }

    #endregion

    public TimeSpan RefreshTokenLifeTime =>
        new TimeSpan(RefreshTokenLifeTimeInDays, 0, 0,0,0,0);
}