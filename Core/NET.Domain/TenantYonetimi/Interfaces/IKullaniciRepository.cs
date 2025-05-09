using NET.Domain.TenantYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Interfaces
{
    /// <summary>
    /// Kullanıcı repository arayüzü.
    /// </summary>
    public interface IKullaniciRepository
    {
        Task<Kullanici> GetByIdAsync(int id, int tenantId);
        Task<Kullanici> GetByEmailAsync(string email, int tenantId);
        Task<IEnumerable<Kullanici>> GetAllByTenantIdAsync(int tenantId);
        Task<Kullanici> AddAsync(Kullanici kullanici);
        Task<bool> UpdateAsync(Kullanici kullanici);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsByIdAsync(int id, int tenantId);
        Task<bool> ExistsByEmailAsync(string email, int tenantId);
        Task<int> GetKullaniciCountByTenantIdAsync(int tenantId);

        // Kimlik doğrulama ve şifre sıfırlama işlemleri için ek metotlar
        Task<Kullanici> AuthenticateAsync(string email, string hashedPassword, int tenantId);
        Task<bool> ResetPasswordAsync(int id, string yeniSifreHash, int tenantId);
        Task<bool> CreatePasswordResetTokenAsync(string email, string token, int tenantId);
        Task<Kullanici> GetByPasswordResetTokenAsync(string token, int tenantId);
    }
}
