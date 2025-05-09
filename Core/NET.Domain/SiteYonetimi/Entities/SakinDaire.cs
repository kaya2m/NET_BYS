using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.SiteYonetimi.Entities
{
    /// <summary>
    /// Sakin-Daire ilişkisi entity'si.
    /// Sakinlerin hangi dairelerde, hangi tarihler arasında kaldığını belirtir.
    /// </summary>
    public class SakinDaire : TenantEntity
    {
        public int SakinId { get; private set; }
        public int DaireId { get; private set; }
        public DateTime BaslangicTarihi { get; private set; }
        public DateTime? BitisTarihi { get; private set; }
        public bool Aktif { get; private set; }

        // ORM için boş constructor
        private SakinDaire() { }

        public SakinDaire(int tenantId, int sakinId, int daireId, DateTime baslangicTarihi)
            : base(tenantId)
        {
            if (sakinId <= 0)
                throw new ArgumentException("Sakin ID geçerli değil", nameof(sakinId));

            if (daireId <= 0)
                throw new ArgumentException("Daire ID geçerli değil", nameof(daireId));

            SakinId = sakinId;
            DaireId = daireId;
            BaslangicTarihi = baslangicTarihi;
            Aktif = true;
        }

        public void UpdateBaslangicTarihi(DateTime baslangicTarihi)
        {
            BaslangicTarihi = baslangicTarihi;
        }

        public void SetBitisTarihi(DateTime bitisTarihi)
        {
            if (bitisTarihi < BaslangicTarihi)
                throw new ArgumentException("Bitiş tarihi başlangıç tarihinden önce olamaz", nameof(bitisTarihi));

            BitisTarihi = bitisTarihi;
            Aktif = false;
        }

        public void SetAktif(bool aktif)
        {
            Aktif = aktif;

            if (!aktif && !BitisTarihi.HasValue)
            {
                BitisTarihi = DateTime.UtcNow;
            }
        }
    }
}
