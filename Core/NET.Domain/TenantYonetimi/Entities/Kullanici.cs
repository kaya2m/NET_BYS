using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Entities
{
    /// <summary>
    /// Kullanıcı entity'si.
    /// Sistemde oturum açabilen kullanıcıları temsil eder.
    /// </summary>
    public class Kullanici : TenantEntity
    {
        public string Ad { get; private set; }
        public string Soyad { get; private set; }
        public string Email { get; private set; }
        public string TelefonNo { get; private set; }
        public string Sifre { get; private set; } // Hash'lenmiş şifre
        public int? RolId { get; private set; }
        public DateTime? SonGirisTarihi { get; private set; }
        public bool Aktif { get; private set; }
        public string PasswordResetToken { get; private set; }
        public DateTime? PasswordResetExpires { get; private set; }
        public bool EmailDogrulandiMi { get; private set; }

        // ORM için boş constructor
        private Kullanici() { }

        public Kullanici(int tenantId, string ad, string soyad, string email, string sifre,
                       string telefonNo = null, int? rolId = null)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Ad boş olamaz", nameof(ad));

            if (string.IsNullOrWhiteSpace(soyad))
                throw new ArgumentException("Soyad boş olamaz", nameof(soyad));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email boş olamaz", nameof(email));

            if (string.IsNullOrWhiteSpace(sifre))
                throw new ArgumentException("Şifre boş olamaz", nameof(sifre));

            Ad = ad;
            Soyad = soyad;
            Email = email;
            Sifre = sifre; // Hash'lenmiş olarak geldiği varsayılır
            TelefonNo = telefonNo;
            RolId = rolId;
            Aktif = true;
            EmailDogrulandiMi = false;
        }

        public void UpdateKullaniciInfo(string ad, string soyad, string email, string telefonNo)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            if (!string.IsNullOrWhiteSpace(soyad))
                Soyad = soyad;

            if (!string.IsNullOrWhiteSpace(email) && email != Email)
            {
                Email = email;
                EmailDogrulandiMi = false; // Yeni email doğrulanmadı
            }

            TelefonNo = telefonNo;
        }

        public void SetRol(int? rolId)
        {
            RolId = rolId;
        }

        public void SetSifre(string yeniSifre)
        {
            if (string.IsNullOrWhiteSpace(yeniSifre))
                throw new ArgumentException("Şifre boş olamaz", nameof(yeniSifre));

            Sifre = yeniSifre; // Hash'lenmiş olarak geldiği varsayılır
            PasswordResetToken = null;
            PasswordResetExpires = null;
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;
        }

        public void CreatePasswordResetToken(string token, DateTime expires)
        {
            PasswordResetToken = token;
            PasswordResetExpires = expires;
        }

        public void ClearPasswordResetToken()
        {
            PasswordResetToken = null;
            PasswordResetExpires = null;
        }

        public void SetEmailDogrulandiMi(bool dogrulandi)
        {
            EmailDogrulandiMi = dogrulandi;
        }

        public void UpdateSonGirisTarihi(DateTime sonGirisTarihi)
        {
            SonGirisTarihi = sonGirisTarihi;
        }
    }
}
