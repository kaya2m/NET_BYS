using NET.API.Middlewares;

namespace NET.API.Extensions
{
    /// <summary>
    /// IApplicationBuilder extensions metodları.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Uygulama middleware'lerini ekler.
        /// </summary>
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<TenantMiddleware>();

            return app;
        }

        /// <summary>
        /// Swagger yapılandırmasını ekler.
        /// </summary>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "NET Bina Yönetim API v1");
                options.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
