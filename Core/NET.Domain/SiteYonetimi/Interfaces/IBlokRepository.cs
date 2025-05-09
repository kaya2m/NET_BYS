using NET.Domain.SiteYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Interfaces
{
    /// <summary>
    /// Blok repository arayüzü.
    /// </summary>
    public interface IBlokRepository
    {
        Task<Blok> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Blok>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<Blok> AddAsync(Blok blok);
        Task UpdateAsync(Blok blok);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<int> GetBlokCountBySiteIdAsync(int siteId, int tenantId);
    }
}
