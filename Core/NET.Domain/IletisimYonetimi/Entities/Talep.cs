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
    /// Talep entity'si.
    /// Site/apartman sakinlerinin taleplerini, şikayetlerini, arıza bildirimlerini temsil eder.
    /// </summary>
    public class Talep : TenantEntity, IAggregateRoot
    {
        public int SiteId { get; private set; }
        public int DaireId { get; private set; }
        public int TalepEdenKullaniciId { get; private set; }
        public string TalepTipi { get; private set; } // Şikayet, İstek, Arıza
        public string Baslik { get; private set; }
        public string Aciklama { get; private set; }
        public string Durum { get; private set; } // Beklemede, İşleme Alındı, Tamamlandı, İptal Edildi
        public DateTime OlusturulmaTarihi { get; private set; }
        public DateTime? TamamlanmaTarihi { get; private set; }
        public string OncelikDurumu { get; private set; } // Düşük, Normal, Yüksek, Acil

        // ORM için boş constructor
        private Talep() { }

        public Talep(int tenantId, int siteId, int daireId, int talepEdenKullaniciId,
                   string talepTipi, string baslik, string aciklama, string oncelikDurumu = "Normal")
            : base(tenantId)
        {
            if (siteId <= 0)
                throw new ArgumentException("Site ID geçerli değil", nameof(siteId));

            if (daireId <= 0)
                throw new ArgumentException("Daire ID geçerli değil", nameof(daireId));

            if (talepEdenKullaniciId <= 0)
                throw new ArgumentException("Kullanıcı ID geçerli değil", nameof(talepEdenKullaniciId));

            if (string.IsNullOrWhiteSpace(talepTipi))
                throw new ArgumentException("Talep tipi boş olamaz", nameof(talepTipi));

            if (string.IsNullOrWhiteSpace(baslik))
                throw new ArgumentException("Başlık boş olamaz", nameof(baslik));

            if (string.IsNullOrWhiteSpace(aciklama))
                throw new ArgumentException("Açıklama boş olamaz", nameof(aciklama));

            SiteId = siteId;
            DaireId = daireId;
            TalepEdenKullaniciId = talepEdenKullaniciId;
            TalepTipi = talepTipi;
            Baslik = baslik;
            Aciklama = aciklama;
            Durum = "Beklemede";
            OlusturulmaTarihi = DateTime.UtcNow;
            OncelikDurumu = oncelikDurumu;

            AddDomainEvent(new TalepCreatedEvent(this));
        }

        public void UpdateTalepInfo(string talepTipi, string baslik,
                                  string aciklama, string oncelikDurumu)
        {
            if (Durum == "Tamamlandı" || Durum == "İptal Edildi")
                throw new InvalidOperationException("Tamamlanmış veya iptal edilmiş bir talep güncellenemez");

            if (!string.IsNullOrWhiteSpace(talepTipi))
                TalepTipi = talepTipi;

            if (!string.IsNullOrWhiteSpace(baslik))
                Baslik = baslik;

            if (!string.IsNullOrWhiteSpace(aciklama))
                Aciklama = aciklama;

            if (!string.IsNullOrWhiteSpace(oncelikDurumu))
                OncelikDurumu = oncelikDurumu;
        }

        public void UpdateDurum(string yeniDurum)
        {
            if (Durum == "Tamamlandı" || Durum == "İptal Edildi")
                throw new InvalidOperationException("Tamamlanmış veya iptal edilmiş bir talebin durumu değiştirilemez");

            Durum = yeniDurum;

            if (yeniDurum == "Tamamlandı")
                TamamlanmaTarihi = DateTime.UtcNow;
        }
    }
}
