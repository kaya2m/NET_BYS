using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.Repositories;
using NET.Application.Common.DTOs;
using NET.Application.Common.Interfaces;
using NET.Domain.TenantYonetimi.Interfaces;
using NET.Identity.Helpers;
using NET.Identity.Models;

namespace NET.Identity.Services
{
    /// <summary>
    /// Kimlik doğrulama servisi
    /// Application katmanındaki IIdentityService'in implementasyonu
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IRolRepository _rolRepository;
        private readonly JwtTokenService _jwtTokenService;

        public IdentityService(
            IKullaniciRepository kullaniciRepository,
            IRolRepository rolRepository,
            JwtTokenService jwtTokenService)
        {
            _kullaniciRepository = kullaniciRepository;
            _rolRepository = rolRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password, int tenantId)
        {
            var hashedPassword = PasswordHasher.HashPassword(password);

            var kullanici = await _kullaniciRepository.AuthenticateAsync(email, hashedPassword, tenantId);

            if (kullanici == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Geçersiz kullanıcı adı veya şifre"
                };
            }

            if (!kullanici.Aktif)
            {
                return new AuthenticationResult { Success = false, ErrorMessage = "Kullanıcı hesabınız aktif değil" }   ;
            }

            string rolAdi = null;
            if (kullanici.RolId.HasValue)
            {
                var rol = await _rolRepository.GetByIdAsync(kullanici.RolId.Value, tenantId);
                rolAdi = rol?.Ad;
            }

            var applicationUser = new ApplicationUser
            {
                Id = kullanici.Id,
                TenantId = kullanici.TenantId,
                Email = kullanici.Email,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                RolId = kullanici.RolId,
                RolAdi = rolAdi
            };

            var token = _jwtTokenService.GenerateJwtToken(applicationUser);

            kullanici.UpdateSonGirisTarihi(DateTime.UtcNow);
            await _kullaniciRepository.UpdateAsync(kullanici);

            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                User = new UserDto
                {
                    Id = kullanici.Id,
                    Email = kullanici.Email,
                    Ad = kullanici.Ad,
                    Soyad = kullanici.Soyad,
                    TenantId = kullanici.TenantId,
                    RolId = kullanici.RolId,
                    RolAdi = rolAdi
                }
            };
        }

        /// <summary>
        /// Şifre sıfırlama tokenı oluşturma
        /// </summary>
        public async Task<bool> InitiatePasswordResetAsync(string email, int tenantId)
        {
            var kullanici = await _kullaniciRepository.GetByEmailAsync(email, tenantId);

            if (kullanici == null || !kullanici.Aktif)
                return false;

            var resetToken = PasswordHasher.GenerateResetToken();
            return await _kullaniciRepository.CreatePasswordResetTokenAsync(email, resetToken, tenantId);
        }

        /// <summary>
        /// Token ile şifre sıfırlama
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string resetToken, string newPassword, int tenantId)
        {
            var kullanici = await _kullaniciRepository.GetByPasswordResetTokenAsync(resetToken, tenantId);

            if (kullanici == null)
                return false;

            var hashedPassword = PasswordHasher.HashPassword(newPassword);
            return await _kullaniciRepository.ResetPasswordAsync(kullanici.Id, hashedPassword, tenantId);
        }
    }
}