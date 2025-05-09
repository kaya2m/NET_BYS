using NET.Domain.FinansalYonetim.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Interfaces
{
    /// <summary>
    /// Gider repository arayüzü.
    /// </summary>
    public interface IGiderRepository
    {
        Task<Gider> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Gider>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Gider>> GetAllByGiderTipIdAsync(int giderTipId, int siteId, int tenantId);
        Task<IEnumerable<Gider>> GetOdenmemisGiderlerBySiteIdAsync(int siteId, int tenantId);
        Task<Gider> AddAsync(Gider gider);
        Task UpdateAsync(Gider gider);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<bool> OdemeYapAsync(int giderId, int tenantId, DateTime odemeTarihi);
        Task<decimal> GetTotalGiderTutarBySiteIdAsync(int siteId, int tenantId, DateTime? baslangicTarihi = null, DateTime? bitisTarihi = null);
        Task<decimal> GetTotalOdenenGiderTutarBySiteIdAsync(int siteId, int tenantId, DateTime? baslangicTarihi = null, DateTime? bitisTarihi = null);
    }
}
