using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Application.Common.Interfaces;

namespace NET.API.Controllers.TenantYonetimi
{
    public class AuthController : ApiControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromHeader(Name = "X-TenantId")] int tenantId, [FromBody] LoginRequest request)
        {
            var result = await _identityService.AuthenticateAsync(request.Email, request.Password, tenantId);

            if (!result.Success)
                return Unauthorized(new { message = result.ErrorMessage });

            return Ok(new
            {
                result.Token,
                User = new
                {
                    result.User.Id,
                    result.User.Email,
                    result.User.Ad,
                    result.User.Soyad,
                    result.User.TenantId,
                    result.User.RolId,
                    result.User.RolAdi
                }
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromHeader(Name = "X-TenantId")] int tenantId, [FromBody] ForgotPasswordRequest request)
        {
            var result = await _identityService.InitiatePasswordResetAsync(request.Email, tenantId);

            // Güvenlik nedeniyle her zaman başarılı mesajı dön
            return Ok(new { message = "Şifre sıfırlama talimatları e-posta adresinize gönderilmiştir (eğer hesap mevcutsa)" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromHeader(Name = "X-TenantId")] int tenantId, [FromBody] ResetPasswordRequest request)
        {
            var result = await _identityService.ResetPasswordAsync(request.ResetToken, request.NewPassword, tenantId);

            if (!result)
                return BadRequest(new { message = "Geçersiz veya süresi dolmuş token" });

            return Ok(new { message = "Şifreniz başarıyla değiştirildi" });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new
            {
                Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                TenantId = User.FindFirst("tenant_id")?.Value,
                Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}