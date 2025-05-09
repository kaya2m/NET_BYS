using NET.Domain.SiteYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Interfaces
{
    /// <summary>
    /// Site repository arayüzü.
    /// </summary>
    public interface ISiteRepository
    {
        Task<Site> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Site>> GetAllByTenantIdAsync(int tenantId);
        Task<Site> AddAsync(Site site);
        Task UpdateAsync(Site site);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<int> GetSiteCountByTenantIdAsync(int tenantId);
    }
}
