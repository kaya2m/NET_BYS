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
    /// Ödeme entity'si.
    /// Aidat ve gider ödemelerini temsil eder.
    /// </summary>
    public class Odeme : TenantEntity
    {
        public int? AidatId { get; private set; }
        public int? GiderId { get; private set; }
        public DateTime OdemeTarihi { get; private set; }
        public Para Tutar { get; private set; }
        public string OdemeTipi { get; private set; } // Nakit, Kredi Kartı, Havale/EFT
        public string OdemeYapan { get; private set; }
        public string BankaAdi { get; private set; }
        public string BankaReferansNo { get; private set; }
        public string Aciklama { get; private set; }

        // ORM için boş constructor
        private Odeme() { }

        public Odeme(int tenantId, Para tutar, DateTime odemeTarihi, string odemeTipi,
                   string odemeYapan = null, string bankaAdi = null,
                   string bankaReferansNo = null, string aciklama = null)
            : base(tenantId)
        {
            if (tutar == null || tutar.Deger <= 0)
                throw new ArgumentException("Tutar sıfır veya negatif olamaz", nameof(tutar));

            if (string.IsNullOrWhiteSpace(odemeTipi))
                throw new ArgumentException("Ödeme tipi boş olamaz", nameof(odemeTipi));

            Tutar = tutar;
            OdemeTarihi = odemeTarihi;
            OdemeTipi = odemeTipi;
            OdemeYapan = odemeYapan;
            BankaAdi = bankaAdi;
            BankaReferansNo = bankaReferansNo;
            Aciklama = aciklama;
        }

        public void SetAidat(int aidatId)
        {
            if (GiderId.HasValue)
                throw new InvalidOperationException("Bu ödeme zaten bir gider için kullanılmış");

            if (aidatId <= 0)
                throw new ArgumentException("Aidat ID geçerli değil", nameof(aidatId));

            AidatId = aidatId;
        }

        public void SetGider(int giderId)
        {
            if (AidatId.HasValue)
                throw new InvalidOperationException("Bu ödeme zaten bir aidat için kullanılmış");

            if (giderId <= 0)
                throw new ArgumentException("Gider ID geçerli değil", nameof(giderId));

            GiderId = giderId;
        }

        public void UpdateOdemeInfo(Para tutar, DateTime odemeTarihi, string odemeTipi,
                                  string odemeYapan, string bankaAdi,
                                  string bankaReferansNo, string aciklama)
        {
            if (tutar != null && tutar.Deger > 0)
                Tutar = tutar;

            OdemeTarihi = odemeTarihi;

            if (!string.IsNullOrWhiteSpace(odemeTipi))
                OdemeTipi = odemeTipi;

            OdemeYapan = odemeYapan;
            BankaAdi = bankaAdi;
            BankaReferansNo = bankaReferansNo;
            Aciklama = aciklama;
        }
    }
}
