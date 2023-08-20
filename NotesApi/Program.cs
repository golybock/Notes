using NotesApi.RefreshCookieAuthScheme;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using NotesApi.RefreshCookieAuthScheme.CacheService;
using NotesApi.RefreshCookieAuthScheme.Token;
using NotesApi.Services.Auth;
using NotesApi.Services.Note;
using NotesApi.Services.Note.NoteFileManager;
using NotesApi.Services.Note.Tag;
using NotesApi.Services.User;
using NotesApi.Services.User.UserManager;
using Repositories.Repositories.Note;
using Repositories.Repositories.Note.NoteImage;
using Repositories.Repositories.Note.NoteType;
using Repositories.Repositories.Note.ShareNote;
using Repositories.Repositories.Note.Tag;
using Repositories.Repositories.User;

// todo get from appsettings.json
// todo clear
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

void ConfigureAuth(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(RefreshCookieDefaults.AuthenticationScheme)
        .AddRefreshCookie(
            RefreshCookieDefaults.AuthenticationScheme,
            RefreshCookieDefaults.AuthenticationScheme,
            _ => GetOptions(configuration));

    // auth options
    services.AddSingleton<RefreshCookieOptions>(_ => GetOptions(configuration));
}

void ConfigureRepositories(IServiceCollection services)
{
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<INoteRepository, NoteRepository>();
    services.AddScoped<ITagRepository, TagRepository>();
    services.AddScoped<IShareNotesRepository, ShareNoteRepository>();
    services.AddScoped<INoteTypeRepository, NoteTypeRepository>();
    services.AddScoped<INoteImageRepository, NoteImageRepository>();
}

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<INoteService, NoteService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ITagService, TagService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<INoteFileManager, NoteFileManager>();

    // auth
    services.AddScoped<IAuthManager, AuthManager>();
    services.AddScoped<IUserManager, UserManager>();
    services.AddScoped<ITokenCacheService, TokenCacheService>();
    services.AddScoped<ITokenManager, TokenManager>();
}

void ConfigureCors(IServiceCollection services)
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

void ConfigureRedis(IServiceCollection services)
{
    // add caching
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "127.0.0.1:6379";
        options.InstanceName = "notes:";
    });
}

var builder = WebApplication.CreateBuilder(args);

ConfigureCors(builder.Services);

ConfigureRepositories(builder.Services);

ConfigureServices(builder.Services);

ConfigureAuth(builder.Services, builder.Configuration);

ConfigureRedis(builder.Services);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

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