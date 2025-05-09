using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NET.Application.Common.Interfaces;

namespace NET.Identity.Services
{
    /// <summary>
    /// HTTP isteği üzerinden mevcut kullanıcı bilgilerini sağlayan servis
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return !string.IsNullOrEmpty(userId) && int.TryParse(userId, out var id) ? id : null;
            }
        }

        public int? TenantId
        {
            get
            {
                var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("tenant_id");
                return !string.IsNullOrEmpty(tenantId) && int.TryParse(tenantId, out var id) ? id : null;
            }
        }

        public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public string Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role)
        {
            return string.Equals(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role), role, StringComparison.OrdinalIgnoreCase);
        }
    }
}