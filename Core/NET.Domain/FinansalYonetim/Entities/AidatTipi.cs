using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Entities
{
    /// <summary>
    /// Aidat Tipi entity'si.
    /// Aidat kategorilerini temsil eder (ör. Normal Aidat, Özel Aidat, Havuz Aidatı, vs).
    /// </summary>
    public class AidatTipi : TenantEntity
    {
        public string Ad { get; private set; }
        public string Aciklama { get; private set; }

        // ORM için boş constructor
        private AidatTipi() { }

        public AidatTipi(int tenantId, string ad, string aciklama = null)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Aidat tipi adı boş olamaz", nameof(ad));

            Ad = ad;
            Aciklama = aciklama;
        }

        public void UpdateAidatTipiInfo(string ad, string aciklama)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            Aciklama = aciklama;
        }
    }
}
