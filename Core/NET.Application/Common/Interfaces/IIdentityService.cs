using System.Threading.Tasks;
using NET.Application.Common.DTOs;

namespace NET.Application.Common.Interfaces
{
    /// <summary>
    /// Kimlik doğrulama servisi arayüzü
    /// </summary>
    public interface IIdentityService
    {
        Task<AuthenticationResult> AuthenticateAsync(string email, string password, int tenantId);
        Task<bool> InitiatePasswordResetAsync(string email, int tenantId);
        Task<bool> ResetPasswordAsync(string resetToken, string newPassword, int tenantId);
    }
}