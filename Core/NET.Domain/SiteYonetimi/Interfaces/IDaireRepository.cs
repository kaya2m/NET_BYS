using NET.Domain.SiteYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Interfaces
{
    /// <summary>
    /// Daire repository arayüzü.
    /// </summary>
    public interface IDaireRepository
    {
        Task<Daire> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Daire>> GetAllByBlokIdAsync(int blokId, int tenantId);
        Task<IEnumerable<Daire>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Daire>> GetBosDairelerByBlokIdAsync(int blokId, int tenantId);
        Task<IEnumerable<Daire>> GetDoluDairelerByBlokIdAsync(int blokId, int tenantId);
        Task<Daire> AddAsync(Daire daire);
        Task UpdateAsync(Daire daire);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<int> GetDaireCountByBlokIdAsync(int blokId, int tenantId);
        Task<int> GetDaireCountBySiteIdAsync(int siteId, int tenantId);
    }
}
