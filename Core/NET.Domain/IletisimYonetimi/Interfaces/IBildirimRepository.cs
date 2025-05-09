using NET.Domain.IletisimYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.IletisimYonetimi.Interfaces
{
    /// <summary>
    /// Bildirim repository arayüzü.
    /// </summary>
    public interface IBildirimRepository
    {
        Task<Bildirim> GetByIdAsync(int id, int tenantId);
        Task<IEnumerable<Bildirim>> GetAllByKullaniciIdAsync(int kullaniciId, int tenantId);
        Task<IEnumerable<Bildirim>> GetOkunmamisBildirimlerByKullaniciIdAsync(int kullaniciId, int tenantId);
        Task<Bildirim> AddAsync(Bildirim bildirim);
        Task<bool> MarkAsReadAsync(int id, int tenantId);
        Task<bool> MarkAllAsReadAsync(int kullaniciId, int tenantId);
        Task<bool> DeleteAsync(int id, int tenantId);
        Task<bool> ExistsAsync(int id, int tenantId);
        Task<int> GetOkunmamisBildirimSayisiAsync(int kullaniciId, int tenantId);
    }
}
