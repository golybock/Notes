using Microsoft.AspNetCore.Authorization;
using NotesApi.RefreshCookieAuthScheme;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using NotesApi.RefreshCookieAuthScheme.CacheService;
using NotesApi.RefreshCookieAuthScheme.Cookie;
using NotesApi.RefreshCookieAuthScheme.Token;
using NotesApi.Services.Auth;
using NotesApi.Services.Interfaces.Note;
using NotesApi.Services.Interfaces.Note.Tag;
using NotesApi.Services.Interfaces.User;
using NotesApi.Services.Note;
using NotesApi.Services.Note.Tag;
using NotesApi.Services.User;
using Repositories.Interfaces.User;
using Repositories.Repositories.User;

void SetRefreshCookieAuth(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(RefreshCookieDefaults.AuthenticationScheme)
        .AddRefreshCookie(
            RefreshCookieDefaults.AuthenticationScheme,
            options =>
            {
                options.ConnectionString = configuration.GetConnectionString("notes")!;
                options.Secret = configuration["JWT:Secret"];
                options.TokenLifeTimeInMinutes = int.Parse(configuration["JWT:TokenValidityInMinutes"]!);
                options.RefreshTokenLifeTimeInDays = Int32.Parse(configuration["JWT:RefreshTokenValidityInDays"]!);
                options.ValidIssuer = configuration["JWT:ValidIssuer"];
                options.ValidAudience = configuration["JWT:ValidAudience"];
                options.ValidateIssuer = true;
                options.ValidateAudience = true;
                options.ValidateIssuerSigningKey = true;
                options.ValidateLifetime = true;
            });

    services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(RefreshCookieDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
    });
}

void SetServices(IServiceCollection services)
{
    services.AddScoped<INoteService, NoteService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ITagService, TagService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<ITokenCacheService, TokenCacheService>();
    services.AddScoped<IAuthManager, AuthManager>();
    services.AddScoped<ITokenRepository, TokensRepository>();
    services.AddScoped<ICookieManager, CookieManager>();
    services.AddScoped<ITokenManager, TokenManager>();
}

void SetCors(IServiceCollection services)
{
    // Default Policy
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            corsPolicyBuilder =>
            {
                corsPolicyBuilder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });
}

var builder = WebApplication.CreateBuilder(args);

// добавление кэширования
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "127.0.0.1:6379";
    options.InstanceName = "local";
});

SetRefreshCookieAuth(builder.Services, builder.Configuration);

SetCors(builder.Services);

SetServices(builder.Services);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();