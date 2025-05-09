using Microsoft.AspNetCore.Http;
using NET.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NET.Infrastructure.MultiTenant
{
    /// <summary>
    /// HTTP isteklerinden tenant bilgilerini çıkaran middleware.
    /// </summary>
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICurrentTenantService _currentTenantService;

        public TenantMiddleware(RequestDelegate next, ICurrentTenantService currentTenantService)
        {
            _next = next;
            _currentTenantService = currentTenantService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-TenantId", out var tenantIdHeader))
            {
                if (int.TryParse(tenantIdHeader, out var tenantId) && tenantId > 0)
                {
                    _currentTenantService.SetTenant(tenantId);
                }
            }
            else if (context.Request.Headers.TryGetValue("tenantId", out var tenantIdRoute))
            {
                if (int.TryParse(tenantIdRoute.ToString(), out var tenantId) && tenantId > 0)
                {
                    _currentTenantService.SetTenant(tenantId);
                }
            }

            try
            {
                await _next(context);
            }
            finally
            {
                _currentTenantService.ClearCurrentTenant();
            }
        }
    }
}
