using NET.Domain.Common;
using NET.Domain.TenantYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Events
{
    /// <summary>
    /// Yeni bir tenant oluşturulduğunda fırlatılan event.
    /// </summary>
    public class TenantCreatedEvent : DomainEvent
    {
        public Tenant Tenant { get; }

        public TenantCreatedEvent(Tenant tenant)
        {
            Tenant = tenant;
        }
    }
}
