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
    /// Yeni bir site oluşturulduğunda fırlatılan event.
    /// </summary>
    public class SiteCreatedEvent : DomainEvent
    {
        public Site Site { get; }

        public SiteCreatedEvent(Site site)
        {
            Site = site;
        }
    }
}
