using Microsoft.OpenApi.Models;

namespace NET.API.Extensions
{
    /// <summary>
    /// IServiceCollection extensions metodları.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Swagger yapılandırmasını ekler.
        /// </summary>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NET Bina Yönetim API",
                    Version = "v1",
                    Description = "NET Bina Yönetim Sistemi için REST API"
                });

                // API endpoint'lerinde tenant doğrulaması için JWT yetkilendirme ekle
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Yetkilendirme header'ı. Örnek: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
