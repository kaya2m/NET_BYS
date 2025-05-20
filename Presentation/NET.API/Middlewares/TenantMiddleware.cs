using NET.Domain.TenantYonetimi.Interfaces;

namespace NET.API.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantRepository tenantRepository)
        {
            string host = context.Request.Host.Host;
            var tenant = await tenantRepository.GetByHostAsync(host);

            if (tenant != null)
            {
                context.Items["TenantId"] = tenant.Id;
            }

            await _next(context);
        }
    }
}
