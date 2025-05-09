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
    /// Yeni bir duyuru oluşturulduğunda fırlatılan event.
    /// </summary>
    public class DuyuruCreatedEvent : DomainEvent
    {
        public Duyuru Duyuru { get; }

        public DuyuruCreatedEvent(Duyuru duyuru)
        {
            Duyuru = duyuru;
        }
    }

}
