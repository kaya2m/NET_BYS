using NET.Domain.Common;
using NET.Domain.IletisimYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.IletisimYonetimi.Events
{
    /// <summary>
    /// Yeni bir talep oluşturulduğunda fırlatılan event.
    /// </summary>
    public class TalepCreatedEvent : DomainEvent
    {
        public Talep Talep { get; }

        public TalepCreatedEvent(Talep talep)
        {
            Talep = talep;
        }
    }
}
