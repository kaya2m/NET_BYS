using NET.Domain.Common;
using NET.Domain.FinansalYonetim.Events;
using NET.Domain.FinansalYonetim.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.Entities
{
    /// <summary>
    /// Aidat entity'si.
    /// Dairelerin ödemesi gereken aidatları temsil eder.
    /// </summary>
    public class Aidat : TenantEntity, IAggregateRoot
    {
        public int SiteId { get; private set; }
        public int? AidatTipId { get; private set; }
        public string Donem { get; private set; } // YYYY-MM formatında
        public int DaireId { get; private set; }
        public Para Tutar { get; private set; }
        public DateTime SonOdemeTarihi { get; private set; }
        public bool OdendiMi { get; private set; }
        public DateTime? OdemeTarihi { get; private set; }
        public bool GecikmeZammiUygula { get; private set; }
        public string Aciklama { get; private set; }

        // ORM için boş constructor
        private Aidat() { }

        public Aidat(int tenantId, int siteId, int? aidatTipId, string donem, int daireId,
                   Para tutar, DateTime sonOdemeTarihi, bool gecikmeZammiUygula = true,
                   string aciklama = null)
            : base(tenantId)
        {
            if (siteId <= 0)
                throw new ArgumentException("Site ID geçerli değil", nameof(siteId));

            if (string.IsNullOrWhiteSpace(donem))
                throw new ArgumentException("Dönem bilgisi boş olamaz", nameof(donem));

            if (daireId <= 0)
                throw new ArgumentException("Daire ID geçerli değil", nameof(daireId));

            if (tutar == null || tutar.Deger <= 0)
                throw new ArgumentException("Tutar sıfır veya negatif olamaz", nameof(tutar));

            SiteId = siteId;
            AidatTipId = aidatTipId;
            Donem = donem;
            DaireId = daireId;
            Tutar = tutar;
            SonOdemeTarihi = sonOdemeTarihi;
            GecikmeZammiUygula = gecikmeZammiUygula;
            Aciklama = aciklama;
            OdendiMi = false;

            AddDomainEvent(new AidatCreatedEvent(this));
        }

        public void UpdateAidatInfo(int? aidatTipId, string donem, Para tutar,
                                  DateTime sonOdemeTarihi, bool gecikmeZammiUygula,
                                  string aciklama)
        {
            if (OdendiMi)
                throw new InvalidOperationException("Ödenmiş bir aidat güncellenemez");

            AidatTipId = aidatTipId;

            if (!string.IsNullOrWhiteSpace(donem))
                Donem = donem;

            if (tutar != null && tutar.Deger > 0)
                Tutar = tutar;

            SonOdemeTarihi = sonOdemeTarihi;
            GecikmeZammiUygula = gecikmeZammiUygula;
            Aciklama = aciklama;
        }

        public void MakePayment(DateTime odemeTarihi)
        {
            if (OdendiMi)
                throw new InvalidOperationException("Bu aidat zaten ödenmiş");

            OdendiMi = true;
            OdemeTarihi = odemeTarihi;

            AddDomainEvent(new OdemeAlindiEvent(this));
        }

        public void CancelPayment()
        {
            if (!OdendiMi)
                throw new InvalidOperationException("Bu aidat henüz ödenmemiş");

            OdendiMi = false;
            OdemeTarihi = null;
        }

        public int GetGecikmeSuresi(DateTime currentDate)
        {
            if (OdendiMi)
                return 0;

            if (currentDate <= SonOdemeTarihi)
                return 0;

            return (int)(currentDate - SonOdemeTarihi).TotalDays;
        }

        public Para CalculateGecikmeZammi(DateTime currentDate, decimal gunlukGecikmeOrani)
        {
            if (!GecikmeZammiUygula || OdendiMi || currentDate <= SonOdemeTarihi)
                return new Para(0, Tutar.ParaBirimi);

            int gecikmeGun = GetGecikmeSuresi(currentDate);
            decimal gecikmeZammi = Tutar.Deger * gecikmeGun * (gunlukGecikmeOrani / 100);

            return new Para(Math.Round(gecikmeZammi, 2), Tutar.ParaBirimi);
        }
    }
}
