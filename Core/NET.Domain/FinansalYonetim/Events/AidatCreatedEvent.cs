using NET.Domain.Common;
using NET.Domain.FinansalYonetim.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Events
{
    /// <summary>
    /// Yeni bir aidat oluşturulduğunda fırlatılan event.
    /// </summary>
    public class AidatCreatedEvent : DomainEvent
    {
        public Aidat Aidat { get; }

        public AidatCreatedEvent(Aidat aidat)
        {
            Aidat = aidat;
        }
    }
}
