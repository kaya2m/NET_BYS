using NET.Domain.Common;
using NET.Domain.SiteYonetimi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Events
{
    /// <summary>
    /// Bir daire kiralandığında veya satın alındığında fırlatılan event.
    /// </summary>
    public class DaireKiralandiEvent : DomainEvent
    {
        public Daire Daire { get; }

        public DaireKiralandiEvent(Daire daire)
        {
            Daire = daire;
        }
    }
}
