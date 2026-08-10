using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Infrastructure.Authentication;
using ECommerceOrderManagement.Infrastructure.Contexts;
using ECommerceOrderManagement.Infrastructure.Identity;
using ECommerceOrderManagement.Infrastructure.Orders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerceOrderManagement.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");

        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsHistoryTable("__EFMigrationsHistory_Identity");
            });
        });


        services.AddIdentityCore<ApplicationUser>(
            options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
           .AddRoles<ApplicationRole>()
           .AddSignInManager()
           .AddEntityFrameworkStores<IdentityDbContext>();

        services.AddOptions<JwtOptions>().Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(
            options => !string.IsNullOrWhiteSpace(options.Issuer), "JWT issuer is required.")
            .Validate(
            options => !string.IsNullOrWhiteSpace(options.Audience), "JWT audience is required.")
            .Validate(
            options => !string.IsNullOrWhiteSpace(options.SecretKey), "JWT secret key is required.")
            .Validate(
            options => options.ExpirationMinutes > 0, "JWT expiration must be greater than zero.")
            .ValidateOnStart();

        var jwtSection = configuration.GetSection(JwtOptions.SectionName);

        var issuer = jwtSection[nameof(JwtOptions.Issuer)] ?? throw new InvalidOperationException("JWT issuer is required.");

        var audience = jwtSection[nameof(JwtOptions.Audience)] ?? throw new InvalidOperationException("JWT audience is required.");

        var secretKey = jwtSection[nameof(JwtOptions.SecretKey)] ?? throw new InvalidOperationException("JWT secret key is required.");

        services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();
        services.AddHttpContextAccessor();


        services.AddSingleton<IOrderNumberGenerator, OrderNumberGenerator>();
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}