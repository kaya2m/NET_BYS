using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NET.Application.Common.Interfaces;
using NET.Identity.Models;
using NET.Identity.Services;
using System;
using System.Security.Claims;
using System.Text;

namespace NET.Identity
{
    /// <summary>
    /// Identity katmanı servislerini yapılandırır
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT ayarlarını yapılandır
            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);

            var key = Encoding.ASCII.GetBytes(jwtSettingsSection["SecretKey"]);

            // JWT kimlik doğrulama ekle
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; 
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettingsSection["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettingsSection["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role
                };
            });

            // HttpContext erişimi için
            services.AddHttpContextAccessor();

            // Servis kayıtları
            services.AddSingleton<JwtTokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddAuthorization();
            return services;
        }
    }
}