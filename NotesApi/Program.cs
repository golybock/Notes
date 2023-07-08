using Microsoft.AspNetCore.Authorization;
using NotesApi.RefreshCookieAuthScheme;

void SetRefreshCookieAuth(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(RefreshCookieDefaults.AuthenticationScheme)
        .AddRefreshCookie(
            RefreshCookieDefaults.AuthenticationScheme,
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

SetRefreshCookieAuth(builder.Services, builder.Configuration);

SetCors(builder.Services);

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