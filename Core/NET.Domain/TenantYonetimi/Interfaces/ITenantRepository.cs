using NET.Domain.TenantYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Interfaces
{
    /// <summary>
    /// Tenant repository arayüzü.
    /// </summary>
    public interface ITenantRepository
    {
        Task<Tenant> GetByIdAsync(int id);
        Task<Tenant> GetByHostAsync(string host);
        Task<Tenant> GetByApiKeyAsync(string apiKey);
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<Tenant> AddAsync(Tenant tenant);
        Task<bool> UpdateAsync(Tenant tenant);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByHostAsync(string host);
    }
}
