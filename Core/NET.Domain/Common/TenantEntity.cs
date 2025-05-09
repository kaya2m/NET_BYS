using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.Common
{
    /// <summary>
    /// Multi-tenant yapı için temel entity. Tüm tenant-specific entity'ler bu sınıftan türemelidir.
    /// </summary>
    public abstract class TenantEntity : BaseEntity
    {
        public int TenantId { get; private set; }

        protected TenantEntity(int tenantId)
        {
            if (tenantId <= 0)
                throw new ArgumentException("Tenant ID must be greater than zero", nameof(tenantId));

            TenantId = tenantId;
        }

        // ORM için boş constructor
        protected TenantEntity()
        {
        }

        // Tenant Id güncelleme (özel durumlarda)
        protected void UpdateTenantId(int newTenantId)
        {
            if (newTenantId <= 0)
                throw new ArgumentException("Tenant ID must be greater than zero", nameof(newTenantId));

            TenantId = newTenantId;
        }
    }
}
