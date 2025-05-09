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
    /// Yeni bir kullanıcı oluşturulduğunda fırlatılan event.
    /// </summary>
    public class KullaniciCreatedEvent : DomainEvent
    {
        public Kullanici Kullanici { get; }

        public KullaniciCreatedEvent(Kullanici kullanici)
        {
            Kullanici = kullanici;
        }
    }
}
