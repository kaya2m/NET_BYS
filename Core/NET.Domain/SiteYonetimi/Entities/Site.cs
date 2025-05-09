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
    /// Site entity'si.
    /// Bir site/apartman/toplu konut yerleşkesini temsil eder.
    /// </summary>
    public class Site : TenantEntity, IAggregateRoot
    {
        public string Ad { get; private set; }
        public string Il { get; private set; }
        public string Ilce { get; private set; }
        public string Mahalle { get; private set; }
        public string Sokak { get; private set; }
        public string Cadde { get; private set; }
        public string No { get; private set; }
        public string PostaKodu { get; private set; }
        public string TamAdres { get; private set; }
        public decimal? Enlem { get; private set; }
        public decimal? Boylam { get; private set; }
        public int ToplamBlokSayisi { get; private set; }
        public int ToplamDaireSayisi { get; private set; }
        public string YoneticiAdi { get; private set; }
        public string YoneticiTelefon { get; private set; }
        public string YoneticiEmail { get; private set; }
        public string Aciklama { get; private set; }
        public bool Aktif { get; private set; }

        // ORM için boş constructor
        private Site() { }

        public Site(int tenantId, string ad, string il, string ilce, string mahalle,
                   string no, string yoneticiAdi, string yoneticiTelefon = null,
                   string yoneticiEmail = null, string aciklama = null)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Site adı boş olamaz", nameof(ad));

            Ad = ad;
            Il = il;
            Ilce = ilce;
            Mahalle = mahalle;
            No = no;
            YoneticiAdi = yoneticiAdi;
            YoneticiTelefon = yoneticiTelefon;
            YoneticiEmail = yoneticiEmail;
            Aciklama = aciklama;
            Aktif = true;
            ToplamBlokSayisi = 0;
            ToplamDaireSayisi = 0;

            AddDomainEvent(new SiteCreatedEvent(this));
        }

        public void UpdateSiteInfo(string ad, string il, string ilce, string mahalle,
                                  string no, string yoneticiAdi, string yoneticiTelefon,
                                  string yoneticiEmail, string aciklama)
        {
            Ad = ad;
            Il = il;
            Ilce = ilce;
            Mahalle = mahalle;
            No = no;
            YoneticiAdi = yoneticiAdi;
            YoneticiTelefon = yoneticiTelefon;
            YoneticiEmail = yoneticiEmail;
            Aciklama = aciklama;
        }

        public void SetAdres(string sokak, string cadde, string postaKodu, string tamAdres = null)
        {
            Sokak = sokak;
            Cadde = cadde;
            PostaKodu = postaKodu;
            TamAdres = tamAdres ?? $"{Mahalle} Mah. {(string.IsNullOrEmpty(Cadde) ? "" : Cadde + " Cad. ")}{(string.IsNullOrEmpty(Sokak) ? "" : Sokak + " Sok. ")}No: {No} {Ilce}/{Il}";
        }

        public void SetKoordinatlar(decimal enlem, decimal boylam)
        {
            Enlem = enlem;
            Boylam = boylam;
        }

        public void UpdateBlokSayisi(int blokSayisi)
        {
            ToplamBlokSayisi = blokSayisi;
        }

        public void UpdateDaireSayisi(int daireSayisi)
        {
            ToplamDaireSayisi = daireSayisi;
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;
        }
    }
}
