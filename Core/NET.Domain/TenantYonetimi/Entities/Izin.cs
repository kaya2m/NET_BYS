using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Entities
{
    /// <summary>
    /// İzin entity'si.
    /// Sistemdeki yapılabilecek işlemler için izinleri temsil eder.
    /// </summary>
    public class Izin : TenantEntity
    {
        public string Ad { get; private set; }
        public string Kod { get; private set; }
        public string Aciklama { get; private set; }

        // ORM için boş constructor
        private Izin() { }

        public Izin(int tenantId, string ad, string kod, string aciklama = null)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("İzin adı boş olamaz", nameof(ad));

            if (string.IsNullOrWhiteSpace(kod))
                throw new ArgumentException("İzin kodu boş olamaz", nameof(kod));

            Ad = ad;
            Kod = kod;
            Aciklama = aciklama;
        }

        public void UpdateIzinInfo(string ad, string kod, string aciklama)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            if (!string.IsNullOrWhiteSpace(kod))
                Kod = kod;

            Aciklama = aciklama;
        }
    }
}
