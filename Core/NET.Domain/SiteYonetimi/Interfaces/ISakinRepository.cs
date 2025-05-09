using NET.Domain.SiteYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Interfaces
{
    /// <summary>
    /// Sakin repository arayüzü.
    /// </summary>
    public interface ISakinRepository
    {
        Task<Sakin> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Sakin>> GetAllByTenantIdAsync(int tenantId);
        Task<IEnumerable<Sakin>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Sakin>> GetAllByDaireIdAsync(int daireId, int tenantId);
        Task<Sakin> GetCurrentSakinByDaireIdAsync(int daireId, int tenantId);
        Task<Sakin> AddAsync(Sakin sakin);
        Task UpdateAsync(Sakin sakin);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<bool> ExistsByTCKNAsync(string tckn, int tenantId);
        Task<bool> SetSakinDaireAsync(SakinDaire sakinDaire);
        Task<bool> EndSakinDaireAsync(int sakinDaireId, int tenantId, DateTime? bitisTarihi = null);
        Task<IEnumerable<SakinDaire>> GetSakinDaireHistoryByDaireIdAsync(int daireId, int tenantId);
        Task<IEnumerable<SakinDaire>> GetSakinDaireHistoryBySakinIdAsync(int sakinId, int tenantId);
    }
}
