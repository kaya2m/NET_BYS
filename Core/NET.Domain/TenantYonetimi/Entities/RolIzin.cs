using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.TenantYonetimi.Entities
{
    /// <summary>
    /// Rol-İzin ilişkisi entity'si.
    /// Rollerin hangi izinlere sahip olduğunu belirtir.
    /// </summary>
    public class RolIzin : TenantEntity
    {
        public int RolId { get; private set; }
        public int IzinId { get; private set; }

        // ORM için boş constructor
        private RolIzin() { }

        public RolIzin(int tenantId, int rolId, int izinId)
            : base(tenantId)
        {
            if (rolId <= 0)
                throw new ArgumentException("Rol ID geçerli değil", nameof(rolId));

            if (izinId <= 0)
                throw new ArgumentException("İzin ID geçerli değil", nameof(izinId));

            RolId = rolId;
            IzinId = izinId;
        }
    }
}
