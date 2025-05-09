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
    /// Bir aidat ödemesi alındığında fırlatılan event.
    /// </summary>
    public class OdemeAlindiEvent : DomainEvent
    {
        public Aidat Aidat { get; }

        public OdemeAlindiEvent(Aidat aidat)
        {
            Aidat = aidat;
        }
    }
}
