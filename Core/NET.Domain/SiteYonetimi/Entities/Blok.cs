using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Entities
{
    /// <summary>
    /// Blok entity'si.
    /// Bir site içindeki bir binayı temsil eder.
    /// </summary>
    public class Blok : TenantEntity
    {
        public int SiteId { get; private set; }
        public string Ad { get; private set; }
        public int KatSayisi { get; private set; }
        public int DaireSayisi { get; private set; }
        public string Aciklama { get; private set; }
        public bool Aktif { get; private set; }

        // ORM için boş constructor
        private Blok() { }

        public Blok(int tenantId, int siteId, string ad, string aciklama = null)
            : base(tenantId)
        {
            if (siteId <= 0)
                throw new ArgumentException("Site ID geçerli değil", nameof(siteId));

            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Blok adı boş olamaz", nameof(ad));

            SiteId = siteId;
            Ad = ad;
            Aciklama = aciklama;
            KatSayisi = 0;
            DaireSayisi = 0;
            Aktif = true;
        }

        public void UpdateBlokInfo(string ad, string aciklama)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            Aciklama = aciklama;
        }

        public void UpdateKatSayisi(int katSayisi)
        {
            if (katSayisi >= 0)
                KatSayisi = katSayisi;
        }

        public void UpdateDaireSayisi(int daireSayisi)
        {
            if (daireSayisi >= 0)
                DaireSayisi = daireSayisi;
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;
        }
    }
}
