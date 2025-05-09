using NET.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Infrastructure.MultiTenant
{
    /// <summary>
    /// Mevcut tenant bilgisini yöneten servis implementasyonu.
    /// </summary>
    public class CurrentTenantService : ICurrentTenantService
    {
        private static readonly AsyncLocal<TenantInfo> _currentTenant = new AsyncLocal<TenantInfo>();

        public int? TenantId => _currentTenant.Value?.Id;
        public string TenantName => _currentTenant.Value?.Name;
        public bool IsValid => _currentTenant.Value?.Id > 0;

        public void SetTenant(int tenantId)
        {
            if (tenantId <= 0)
                throw new ArgumentException("Tenant ID must be greater than zero", nameof(tenantId));

            _currentTenant.Value = new TenantInfo
            {
                Id = tenantId,
                Name = null
            };
        }

        public void SetTenant(int tenantId, string tenantName)
        {
            if (tenantId <= 0)
                throw new ArgumentException("Tenant ID must be greater than zero", nameof(tenantId));

            _currentTenant.Value = new TenantInfo
            {
                Id = tenantId,
                Name = tenantName
            };
        }

        public void ClearCurrentTenant()
        {
            _currentTenant.Value = null;
        }

        private class TenantInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
