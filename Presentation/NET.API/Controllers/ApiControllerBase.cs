using MediatR;
using Microsoft.AspNetCore.Mvc;
using NET.Application.Common.Interfaces;

namespace NET.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        private ICurrentUserService _currentUserService;
        private ICurrentTenantService _currentTenantService;

        protected ISender Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected ICurrentUserService CurrentUser =>
            _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

        protected ICurrentTenantService CurrentTenant =>
            _currentTenantService ??= HttpContext.RequestServices.GetRequiredService<ICurrentTenantService>();

        protected int GetTenantIdFromRequest()
        {
            if (Request.Headers.TryGetValue("X-TenantId", out var tenantIdHeader) &&
                int.TryParse(tenantIdHeader, out int tenantId))
            {
                return tenantId;
            }

            if (CurrentUser.TenantId  != null)
            {
                return CurrentUser.TenantId;
            }

            return 0;
        }
    }
}