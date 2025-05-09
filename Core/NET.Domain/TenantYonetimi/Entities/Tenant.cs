using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Entities
{
    /// <summary>
    /// Tenant (Site Yönetimi) entity'si.
    /// Multi-tenant yapının temel entity'sidir.
    /// </summary>
    public class Tenant : BaseEntity, IAggregateRoot
    {
        public string Ad { get; private set; }
        public string Host { get; private set; }
        public string ApiKey { get; private set; }
        public string Aciklama { get; private set; }
        public string Logo { get; private set; }
        public DateTime AktiflestirilmeTarihi { get; private set; }
        public DateTime? BitisTarihi { get; private set; }
        public bool Aktif { get; private set; }
        public int MaxKullaniciSayisi { get; private set; }
        public int MaxSiteSayisi { get; private set; }
        public int? MaxDaireSayisi { get; private set; }

        // ORM için boş constructor
        private Tenant() { }

        public Tenant(string ad, string host, string aciklama = null, string logo = null)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Tenant adı boş olamaz", nameof(ad));

            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("Host bilgisi boş olamaz", nameof(host));

            Ad = ad;
            Host = host;
            Aciklama = aciklama;
            Logo = logo;
            ApiKey = Guid.NewGuid().ToString("N");
            AktiflestirilmeTarihi = DateTime.UtcNow;
            Aktif = true;
            MaxKullaniciSayisi = 10;
            MaxSiteSayisi = 1;
        }

        public void UpdateTenantInfo(string ad, string host, string aciklama, string logo)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            if (!string.IsNullOrWhiteSpace(host))
                Host = host;

            Aciklama = aciklama;
            Logo = logo;
        }

        public void SetLimits(int maxKullaniciSayisi, int maxSiteSayisi, int? maxDaireSayisi)
        {
            if (maxKullaniciSayisi > 0)
                MaxKullaniciSayisi = maxKullaniciSayisi;

            if (maxSiteSayisi > 0)
                MaxSiteSayisi = maxSiteSayisi;

            MaxDaireSayisi = maxDaireSayisi;
        }

        public void RegenerateApiKey()
        {
            ApiKey = Guid.NewGuid().ToString("N");
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;

            if (!aktif && !BitisTarihi.HasValue)
                BitisTarihi = DateTime.UtcNow;
            else if (aktif)
                BitisTarihi = null;
        }

        public void ExtendLicense(DateTime bitisTarihi)
        {
            if (bitisTarihi <= DateTime.UtcNow)
                throw new ArgumentException("Bitiş tarihi şu anki zamandan sonra olmalıdır", nameof(bitisTarihi));

            BitisTarihi = bitisTarihi;
            Aktif = true;
        }
    }
}
