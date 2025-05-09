using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.Common
{
    /// <summary>
    /// Domain event'leri için temel sınıf.
    /// </summary>
    public abstract class DomainEvent
    {
        public DateTime Timestamp { get; }

        protected DomainEvent()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
