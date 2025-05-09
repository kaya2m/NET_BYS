using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Entities
{
    /// <summary>
    /// Sakin entity'si.
    /// Bir dairede yaşayan kişiyi temsil eder.
    /// </summary>
    public class Sakin : TenantEntity
    {
        public string Ad { get; private set; }
        public string Soyad { get; private set; }
        public string TCKN { get; private set; }
        public string TelefonNo { get; private set; }
        public string Email { get; private set; }
        public bool MulkSahibiMi { get; private set; }
        public int? KullaniciId { get; private set; }
        public string Aciklama { get; private set; }
        public bool Aktif { get; private set; }

        // ORM için boş constructor
        private Sakin() { }

        public Sakin(int tenantId, string ad, string soyad, string telefonNo = null,
                    string email = null, string tckn = null, bool mulkSahibiMi = false,
                    int? kullaniciId = null, string aciklama = null)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Ad boş olamaz", nameof(ad));

            if (string.IsNullOrWhiteSpace(soyad))
                throw new ArgumentException("Soyad boş olamaz", nameof(soyad));

            Ad = ad;
            Soyad = soyad;
            TCKN = tckn;
            TelefonNo = telefonNo;
            Email = email;
            MulkSahibiMi = mulkSahibiMi;
            KullaniciId = kullaniciId;
            Aciklama = aciklama;
            Aktif = true;
        }

        public void UpdateSakinInfo(string ad, string soyad, string telefonNo,
                                  string email, string tckn, string aciklama)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            if (!string.IsNullOrWhiteSpace(soyad))
                Soyad = soyad;

            TelefonNo = telefonNo;
            Email = email;
            TCKN = tckn;
            Aciklama = aciklama;
        }

        public void SetMulkSahibiMi(bool mulkSahibiMi)
        {
            MulkSahibiMi = mulkSahibiMi;
        }

        public void SetKullaniciId(int? kullaniciId)
        {
            KullaniciId = kullaniciId;
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;
        }

        public string TamAd => $"{Ad} {Soyad}";
    }
}
