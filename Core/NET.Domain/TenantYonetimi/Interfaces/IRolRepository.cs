using NET.Domain.TenantYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Interfaces
{
    /// <summary>
    /// Rol repository arayüzü.
    /// </summary>
    public interface IRolRepository
    {
        Task<Rol> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Rol>> GetAllByTenantIdAsync(int tenantId);
        Task<Rol> AddAsync(Rol rol);
        Task<bool> UpdateAsync(Rol rol);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsByIdAsync(int id, int tenantId);

        // Rol-İzin ilişkileri için ek metotlar
        Task<IEnumerable<Izin>> GetIzinlerByRolIdAsync(int rolId, int tenantId);
        Task<bool> AddRolIzinAsync(RolIzin rolIzin);
        Task<bool> RemoveRolIzinAsync(int rolId, int izinId, int tenantId);
        Task<bool> HasIzinAsync(int rolId, string izinKodu, int tenantId);
    }
}
