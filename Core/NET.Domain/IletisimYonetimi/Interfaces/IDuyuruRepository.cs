using NET.Domain.IletisimYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.IletisimYonetimi.Interfaces
{
    /// <summary>
    /// Duyuru repository arayüzü.
    /// </summary>
    public interface IDuyuruRepository
    {
        Task<Duyuru> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Duyuru>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Duyuru>> GetAktifDuyurularBySiteIdAsync(int siteId, int tenantId, DateTime? date = null);
        Task<IEnumerable<Duyuru>> GetAcilDuyurularBySiteIdAsync(int siteId, int tenantId);
        Task<Duyuru> AddAsync(Duyuru duyuru);
        Task UpdateAsync(Duyuru duyuru);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
    }
}
