using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Identity.Models
{
    /// <summary>
    /// JWT token içinde kullanılacak kullanıcı bilgisi modeli
    /// NET.Domain.TenantYonetimi.Entities.Kullanici entity'sine karşılık gelir
    /// </summary>
    public class ApplicationUser
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Email { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int? RolId { get; set; }
        public string RolAdi { get; set; }
    }
}
