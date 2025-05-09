using NET.Domain.IletisimYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.IletisimYonetimi.Interfaces
{
    /// <summary>
    /// Talep repository arayüzü.
    /// </summary>
    public interface ITalepRepository
    {
        Task<Talep> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Talep>> GetAllBySiteIdAsync(int siteId, int tenantId);
        Task<IEnumerable<Talep>> GetAllByDaireIdAsync(int daireId, int tenantId);
        Task<IEnumerable<Talep>> GetAllByKullaniciIdAsync(int kullaniciId, int tenantId);
        Task<IEnumerable<Talep>> GetByDurumAndSiteIdAsync(string durum, int siteId, int tenantId);
        Task<Talep> AddAsync(Talep talep);
        Task UpdateAsync(Talep talep);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<TalepYaniti> AddTalepYanitiAsync(TalepYaniti talepYaniti);
        Task<IEnumerable<TalepYaniti>> GetYanitlarByTalepIdAsync(int talepId, int tenantId);
        Task<int> GetAcikTalepSayisiAsync(int siteId, int tenantId);
    }
}
