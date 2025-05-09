using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.IletisimYonetimi.Entities
{
    /// <summary>
    /// Talep Yanıtı entity'si.
    /// Taleplere verilen yanıtları temsil eder.
    /// </summary>
    public class TalepYaniti : TenantEntity
    {
        public int TalepId { get; private set; }
        public int YanitVerenKullaniciId { get; private set; }
        public string Yanit { get; private set; }
        public DateTime YanitTarihi { get; private set; }

        // ORM için boş constructor
        private TalepYaniti() { }

        public TalepYaniti(int tenantId, int talepId, int yanitVerenKullaniciId, string yanit)
            : base(tenantId)
        {
            if (talepId <= 0)
                throw new ArgumentException("Talep ID geçerli değil", nameof(talepId));

            if (yanitVerenKullaniciId <= 0)
                throw new ArgumentException("Kullanıcı ID geçerli değil", nameof(yanitVerenKullaniciId));

            if (string.IsNullOrWhiteSpace(yanit))
                throw new ArgumentException("Yanıt boş olamaz", nameof(yanit));

            TalepId = talepId;
            YanitVerenKullaniciId = yanitVerenKullaniciId;
            Yanit = yanit;
            YanitTarihi = DateTime.UtcNow;
        }

        public void UpdateYanit(string yanit)
        {
            if (!string.IsNullOrWhiteSpace(yanit))
                Yanit = yanit;
        }
    }
}
