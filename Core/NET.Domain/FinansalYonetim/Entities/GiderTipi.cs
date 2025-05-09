using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Entities
{
    /// <summary>
    /// Gider Tipi entity'si.
    /// Gider kategorilerini temsil eder (ör. Elektrik, Su, Doğalgaz, Temizlik, vs).
    /// </summary>
    public class GiderTipi : TenantEntity
    {
        public string Ad { get; private set; }
        public string Aciklama { get; private set; }

        // ORM için boş constructor
        private GiderTipi() { }

        public GiderTipi(int tenantId, string ad, string aciklama = null)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Gider tipi adı boş olamaz", nameof(ad));

            Ad = ad;
            Aciklama = aciklama;
        }

        public void UpdateGiderTipiInfo(string ad, string aciklama)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            Aciklama = aciklama;
        }
    }
}
