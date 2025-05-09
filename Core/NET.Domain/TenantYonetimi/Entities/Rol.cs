using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Entities
{
    /// <summary>
    /// Rol entity'si.
    /// Kullanıcıların sistemdeki rollerini temsil eder.
    /// </summary>
    public class Rol : TenantEntity
    {
        public string Ad { get; private set; }
        public string Aciklama { get; private set; }

        // ORM için boş constructor
        private Rol() { }

        public Rol(int tenantId, string ad, string aciklama = null)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                throw new ArgumentException("Rol adı boş olamaz", nameof(ad));

            Ad = ad;
            Aciklama = aciklama;
        }

        public void UpdateRolInfo(string ad, string aciklama)
        {
            if (!string.IsNullOrWhiteSpace(ad))
                Ad = ad;

            Aciklama = aciklama;
        }
    }
}
