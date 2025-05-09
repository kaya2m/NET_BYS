using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NET.Application.Common.Interfaces;

namespace NET.API.Filters
{
    /// <summary>
    /// Tenant doğrulaması için action filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TenantValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tenantService = context.HttpContext.RequestServices.GetService<ICurrentTenantService>();

            if (tenantService == null || !tenantService.IsValid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
