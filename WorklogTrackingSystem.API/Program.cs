using WorklogTrackingSystem.Application.Interfaces;
using WorklogTrackingSystem.Infrastructure.Data;
using WorklogTrackingSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WorklogTrackingSystem.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var allowOrigins = "_allowDevelopmentOrigins";

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4173", "https://localhost:4173", "http://localhost:5666", "https://localhost:5666")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("INFO: Configuring DbContext for Development (SQLite)");
    builder.Services.AddDbContext<SqliteDbContext>(options =>
        options.UseSqlite(
            builder.Configuration.GetConnectionString("SQLiteConnection"),            
            sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Sqlite")
        ));
    builder.Services.AddScoped<IDbContext>(provider => provider.GetRequiredService<SqliteDbContext>());
}
else
{
    Console.WriteLine("INFO: Configuring DbContext for Production (SQL Server)");
    builder.Services.AddDbContext<SqlServerDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("SQLServerConnection"),
            sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_SqlServer")
        ));
    builder.Services.AddScoped<IDbContext>(provider => provider.GetRequiredService<SqlServerDbContext>());
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorklogService, WorklogService>();

var app = builder.Build();

// Seed the database with initial 10000 Users and their Worklogs
using (var scope = app.Services.CreateScope())
{
    if (app.Environment.IsDevelopment())
    {
        var sqliteContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
        var databaseSeeder = new DatabaseSeeder(sqliteContext);
        await databaseSeeder.SeedUsers();
    }
    else
    {
        var sqlServerContext = scope.ServiceProvider.GetRequiredService<SqlServerDbContext>();
        var databaseSeeder = new DatabaseSeeder(sqlServerContext);
        await databaseSeeder.SeedUsers();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(allowOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
