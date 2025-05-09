using NET.Domain.TenantYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Interfaces
{
    /// <summary>
    /// İzin repository arayüzü.
    /// </summary>
    public interface IIzinRepository
    {
        Task<Izin> GetByIdAsync(int id, int tenantId);
        Task<Izin> GetByKodAsync(string kod, int tenantId);
        Task<IEnumerable<Izin>> GetAllByTenantIdAsync(int tenantId);
        Task<Izin> AddAsync(Izin izin);
        Task UpdateAsync(Izin izin);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<bool> ExistsByKodAsync(string kod, int tenantId);
        Task<IEnumerable<Izin>> GetIzinlerByRolIdAsync(int rolId, int tenantId);
    }
}
