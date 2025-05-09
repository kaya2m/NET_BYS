using NET.Domain.FinansalYonetim.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Interfaces
{
    /// <summary>
    /// Ödeme repository arayüzü.
    /// </summary>
    public interface IOdemeRepository
    {
        Task<Odeme> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Odeme>> GetAllByTenantIdAsync(int tenantId);
        Task<IEnumerable<Odeme>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Odeme>> GetAllByAidatIdAsync(int aidatId, int tenantId);
        Task<IEnumerable<Odeme>> GetAllByGiderIdAsync(int giderId, int tenantId);
        Task<Odeme> AddAsync(Odeme odeme);
        Task UpdateAsync(Odeme odeme);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
    }
}
