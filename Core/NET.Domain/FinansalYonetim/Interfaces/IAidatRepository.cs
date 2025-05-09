using NET.Domain.FinansalYonetim.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Interfaces
{
    /// <summary>
    /// Aidat repository arayüzü.
    /// </summary>
    public interface IAidatRepository
    {
        Task<Aidat> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Aidat>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Aidat>> GetAllByDaireIdAsync(int daireId, int tenantId);
        Task<IEnumerable<Aidat>> GetOdenmemisAidatlarBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Aidat>> GetOdenmemisAidatlarByDaireIdAsync(int daireId, int tenantId);
        Task<IEnumerable<Aidat>> GetAidatlarByDonemAsync(string donem, int siteId, int tenantId);
        Task<Aidat> AddAsync(Aidat aidat);
        Task UpdateAsync(Aidat aidat);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<bool> OdemeYapAsync(int aidatId, int tenantId, DateTime odemeTarihi);
        Task<decimal> GetTotalAidatTutarBySiteIdAsync(int siteId, int tenantId, DateTime? baslangicTarihi = null, DateTime? bitisTarihi = null);
        Task<decimal> GetTotalOdenenAidatTutarBySiteIdAsync(int siteId, int tenantId, DateTime? baslangicTarihi = null, DateTime? bitisTarihi = null);
    }
}
