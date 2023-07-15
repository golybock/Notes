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

// todo get from appsettings.json
RefreshCookieOptions GetOptions(IConfiguration configuration)
{
    return new RefreshCookieOptions()
    {
        Secret = configuration["RefreshCookieOptions:Secret"],
        TokenLifeTimeInMinutes = int.Parse(configuration["RefreshCookieOptions:TokenValidityInMinutes"]!),
        RefreshTokenLifeTimeInDays = Int32.Parse(configuration["RefreshCookieOptions:RefreshTokenValidityInDays"]!),
        ValidIssuer = configuration["RefreshCookieOptions:ValidIssuer"],
        ValidAudience = configuration["RefreshCookieOptions:ValidAudience"],
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
}

void SetRefreshCookieAuth(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(RefreshCookieDefaults.AuthenticationScheme)
        .AddRefreshCookie(
            RefreshCookieDefaults.AuthenticationScheme,
            RefreshCookieDefaults.AuthenticationScheme,
            options => GetOptions(configuration));

    services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(RefreshCookieDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
    });
}

void SetOptions(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<RefreshCookieOptions>(configuration.GetSection("RefreshCookieOptions"));
}

void SetServices(IServiceCollection services)
{
    services.AddScoped<INoteService, NoteService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ITagService, TagService>();
    services.AddScoped<IAuthService, AuthService>();
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

void SetRedis(IServiceCollection services)
{
    // добавление кэширования
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "127.0.0.1:6379";
        options.InstanceName = "local";
    });
}

var builder = WebApplication.CreateBuilder(args);

SetRefreshCookieAuth(builder.Services, builder.Configuration);

SetCors(builder.Services);

SetServices(builder.Services);

SetOptions(builder.Services, builder.Configuration);

SetRedis(builder.Services);

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