using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.IletisimYonetimi.Entities
{
    /// <summary>
    /// Bildirim entity'si.
    /// Kullanıcılara iletilen bildirimleri temsil eder.
    /// </summary>
    public class Bildirim : TenantEntity
    {
        public int AliciKullaniciId { get; private set; }
        public string Baslik { get; private set; }
        public string Icerik { get; private set; }
        public DateTime OlusturulmaTarihi { get; private set; }
        public bool OkunduMu { get; private set; }
        public DateTime? OkunmaTarihi { get; private set; }
        public string BildirimTipi { get; private set; } // Duyuru, TalepYaniti, AidatHatirlatma
        public int? IlgiliId { get; private set; } // İlgili Duyuru, Talep veya Aidat Id'si

        // ORM için boş constructor
        private Bildirim() { }

        public Bildirim(int tenantId, int aliciKullaniciId, string baslik, string icerik,
                      string bildirimTipi, int? ilgiliId = null)
            : base(tenantId)
        {
            if (aliciKullaniciId <= 0)
                throw new ArgumentException("Kullanıcı ID geçerli değil", nameof(aliciKullaniciId));

            if (string.IsNullOrWhiteSpace(baslik))
                throw new ArgumentException("Başlık boş olamaz", nameof(baslik));

            if (string.IsNullOrWhiteSpace(icerik))
                throw new ArgumentException("İçerik boş olamaz", nameof(icerik));

            if (string.IsNullOrWhiteSpace(bildirimTipi))
                throw new ArgumentException("Bildirim tipi boş olamaz", nameof(bildirimTipi));

            AliciKullaniciId = aliciKullaniciId;
            Baslik = baslik;
            Icerik = icerik;
            OlusturulmaTarihi = DateTime.UtcNow;
            OkunduMu = false;
            BildirimTipi = bildirimTipi;
            IlgiliId = ilgiliId;
        }

        public void MarkAsRead()
        {
            if (!OkunduMu)
            {
                OkunduMu = true;
                OkunmaTarihi = DateTime.UtcNow;
            }
        }

        public void MarkAsUnread()
        {
            if (OkunduMu)
            {
                OkunduMu = false;
                OkunmaTarihi = null;
            }
        }
    }
}
