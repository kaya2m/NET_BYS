using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Application.Common.Interfaces
{
    /// <summary>
    /// Tarih/zaman işlemleri için arayüz.
    /// Test edilebilirliği artırmak için kullanılır.
    /// </summary>
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
