using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Application.Common.Interfaces
{
    /// <summary>
    /// Mevcut tenant bilgilerine erişim sağlayan servis arayüzü.
    /// </summary>
    public interface ICurrentTenantService
    {
        int? TenantId { get; }
        string TenantName { get; }
        bool IsValid { get; }
        void SetTenant(int tenantId);
        void SetTenant(int tenantId, string tenantName);
        void ClearCurrentTenant();
    }
}
