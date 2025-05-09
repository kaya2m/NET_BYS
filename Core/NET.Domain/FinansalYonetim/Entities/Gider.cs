using NET.Domain.Common;
using NET.Domain.FinansalYonetim.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Entities
{
    /// <summary>
    /// Gider entity'si.
    /// Site/apartman giderlerini temsil eder.
    /// </summary>
    public class Gider : TenantEntity, IAggregateRoot
    {
        public int SiteId { get; private set; }
        public int? GiderTipId { get; private set; }
        public DateTime FaturaTarihi { get; private set; }
        public DateTime SonOdemeTarihi { get; private set; }
        public bool OdendiMi { get; private set; }
        public DateTime? OdemeTarihi { get; private set; }
        public Para Tutar { get; private set; }
        public string FaturaNo { get; private set; }
        public string Aciklama { get; private set; }

        // ORM için boş constructor
        private Gider() { }

        public Gider(int tenantId, int siteId, int? giderTipId, DateTime faturaTarihi,
                   DateTime sonOdemeTarihi, Para tutar, string faturaNo = null,
                   string aciklama = null)
            : base(tenantId)
        {
            if (siteId <= 0)
                throw new ArgumentException("Site ID geçerli değil", nameof(siteId));

            if (tutar == null || tutar.Deger <= 0)
                throw new ArgumentException("Tutar sıfır veya negatif olamaz", nameof(tutar));

            SiteId = siteId;
            GiderTipId = giderTipId;
            FaturaTarihi = faturaTarihi;
            SonOdemeTarihi = sonOdemeTarihi;
            Tutar = tutar;
            FaturaNo = faturaNo;
            Aciklama = aciklama;
            OdendiMi = false;
        }

        public void UpdateGiderInfo(int? giderTipId, DateTime faturaTarihi,
                                 DateTime sonOdemeTarihi, Para tutar,
                                 string faturaNo, string aciklama)
        {
            if (OdendiMi)
                throw new InvalidOperationException("Ödenmiş bir gider güncellenemez");

            GiderTipId = giderTipId;
            FaturaTarihi = faturaTarihi;
            SonOdemeTarihi = sonOdemeTarihi;

            if (tutar != null && tutar.Deger > 0)
                Tutar = tutar;

            FaturaNo = faturaNo;
            Aciklama = aciklama;
        }

        public void MakePayment(DateTime odemeTarihi)
        {
            if (OdendiMi)
                throw new InvalidOperationException("Bu gider zaten ödenmiş");

            OdendiMi = true;
            OdemeTarihi = odemeTarihi;
        }

        public void CancelPayment()
        {
            if (!OdendiMi)
                throw new InvalidOperationException("Bu gider henüz ödenmemiş");

            OdendiMi = false;
            OdemeTarihi = null;
        }
    }
}
