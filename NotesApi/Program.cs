using Microsoft.AspNetCore.Authorization;
using NotesApi.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(AuthSchemeOptions.Name)
    .AddNoOpAuth(
        AuthSchemeOptions.Name,  AuthSchemeOptions.Name, options =>
        {
            options.ConnectionString = builder.Configuration.GetConnectionString("notes")!;
            options.Secret = builder.Configuration["JWT:Secret"];
            options.TokenLifeTimeInMinutes = int.Parse(builder.Configuration["JWT:TokenValidityInMinutes"]);
            options.RefreshTokenLifeTimeInDays = Int32.Parse(builder.Configuration["JWT:RefreshTokenValidityInDays"]);
            options.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
            options.ValidAudience = builder.Configuration["JWT:ValidAudience"];
            options.ValidateLifetime = true;
        });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes("Aboba")
        .RequireAuthenticatedUser()
        .Build();
});

// Default Policy
builder.Services.AddCors(options =>
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

app.Run();