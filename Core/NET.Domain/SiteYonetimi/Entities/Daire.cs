using NET.Domain.Common;
using NET.Domain.SiteYonetimi.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Entities
{
    /// <summary>
    /// Daire entity'si.
    /// Bir blok içindeki bağımsız bölümü temsil eder.
    /// </summary>
    public class Daire : TenantEntity
    {
        public int BlokId { get; private set; }
        public string No { get; private set; }
        public int Kat { get; private set; }
        public int OdaSayisi { get; private set; }
        public decimal Metrekare { get; private set; }
        public string Tip { get; private set; } // 1+1, 2+1, 3+1 gibi
        public bool Dolu { get; private set; }
        public int? SakinId { get; private set; }
        public string Aciklama { get; private set; }
        public bool Aktif { get; private set; }

        // ORM için boş constructor
        private Daire() { }

        public Daire(int tenantId, int blokId, string no, int kat, int odaSayisi,
                    decimal metrekare, string tip, string aciklama = null)
            : base(tenantId)
        {
            if (blokId <= 0)
                throw new ArgumentException("Blok ID geçerli değil", nameof(blokId));

            if (string.IsNullOrWhiteSpace(no))
                throw new ArgumentException("Daire no boş olamaz", nameof(no));

            if (odaSayisi <= 0)
                throw new ArgumentException("Oda sayısı sıfır veya negatif olamaz", nameof(odaSayisi));

            if (metrekare <= 0)
                throw new ArgumentException("Metrekare sıfır veya negatif olamaz", nameof(metrekare));

            BlokId = blokId;
            No = no;
            Kat = kat;
            OdaSayisi = odaSayisi;
            Metrekare = metrekare;
            Tip = tip;
            Aciklama = aciklama;
            Dolu = false;
            Aktif = true;
        }

        public void UpdateDaireInfo(string no, int kat, int odaSayisi, decimal metrekare, string tip, string aciklama)
        {
            if (!string.IsNullOrWhiteSpace(no))
                No = no;

            Kat = kat;

            if (odaSayisi > 0)
                OdaSayisi = odaSayisi;

            if (metrekare > 0)
                Metrekare = metrekare;

            Tip = tip;
            Aciklama = aciklama;
        }

        public void SakinAta(int sakinId)
        {
            if (sakinId <= 0)
                throw new ArgumentException("Sakin ID geçerli değil", nameof(sakinId));

            if (Dolu && SakinId.HasValue && SakinId.Value != sakinId)
                throw new InvalidOperationException("Daire zaten dolu. Önce mevcut sakini çıkarın.");

            SakinId = sakinId;
            Dolu = true;

            AddDomainEvent(new DaireKiralandiEvent(this));
        }

        public void SakinCikar()
        {
            if (!Dolu || !SakinId.HasValue)
                throw new InvalidOperationException("Dairede kayıtlı sakin bulunmuyor.");

            SakinId = null;
            Dolu = false;
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;
        }
    }
}
