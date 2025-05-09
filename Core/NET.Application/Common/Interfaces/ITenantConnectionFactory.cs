using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Application.Common.Interfaces
{
    /// <summary>
    /// Tenant'a özel veritabanı bağlantıları oluşturmak için arayüz.
    /// </summary>
    public interface ITenantConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
