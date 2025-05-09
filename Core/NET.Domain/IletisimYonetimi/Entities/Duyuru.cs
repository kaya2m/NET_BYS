using NET.Domain.Common;
using NET.Domain.IletisimYonetimi.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.IletisimYonetimi.Entities
{
    /// <summary>
    /// Duyuru entity'si.
    /// Site/apartman duyurularını temsil eder.
    /// </summary>
    public class Duyuru : TenantEntity, IAggregateRoot
    {
        public int SiteId { get; private set; }
        public string Baslik { get; private set; }
        public string Icerik { get; private set; }
        public DateTime YayinTarihi { get; private set; }
        public DateTime? BitisTarihi { get; private set; }
        public bool Acil { get; private set; }
        public bool Aktif { get; private set; }

        // ORM için boş constructor
        private Duyuru() { }

        public Duyuru(int tenantId, int siteId, string baslik, string icerik,
                     DateTime yayinTarihi, DateTime? bitisTarihi = null, bool acil = false)
            : base(tenantId)
        {
            if (siteId <= 0)
                throw new ArgumentException("Site ID geçerli değil", nameof(siteId));

            if (string.IsNullOrWhiteSpace(baslik))
                throw new ArgumentException("Başlık boş olamaz", nameof(baslik));

            if (string.IsNullOrWhiteSpace(icerik))
                throw new ArgumentException("İçerik boş olamaz", nameof(icerik));

            SiteId = siteId;
            Baslik = baslik;
            Icerik = icerik;
            YayinTarihi = yayinTarihi;
            BitisTarihi = bitisTarihi;
            Acil = acil;
            Aktif = true;

            AddDomainEvent(new DuyuruCreatedEvent(this));
        }

        public void UpdateDuyuruInfo(string baslik, string icerik, DateTime yayinTarihi,
                                   DateTime? bitisTarihi, bool acil)
        {
            if (!string.IsNullOrWhiteSpace(baslik))
                Baslik = baslik;

            if (!string.IsNullOrWhiteSpace(icerik))
                Icerik = icerik;

            YayinTarihi = yayinTarihi;
            BitisTarihi = bitisTarihi;
            Acil = acil;
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;
        }

        public bool IsActive(DateTime currentDate)
        {
            if (!Aktif)
                return false;

            if (currentDate < YayinTarihi)
                return false;

            if (BitisTarihi.HasValue && currentDate > BitisTarihi.Value)
                return false;

            return true;
        }
    }
}
