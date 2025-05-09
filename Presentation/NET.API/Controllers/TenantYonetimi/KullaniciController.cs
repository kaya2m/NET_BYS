using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Domain.TenantYonetimi.Entities;
using NET.Domain.TenantYonetimi.Interfaces;
using NET.Identity.Helpers;

namespace NET.API.Controllers.TenantYonetimi
{
    [Authorize]
    public class KullaniciController : ApiControllerBase
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IRolRepository _rolRepository;

        public KullaniciController(
            IKullaniciRepository kullaniciRepository,
            IRolRepository rolRepository)
        {
            _kullaniciRepository = kullaniciRepository;
            _rolRepository = rolRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenantId = GetTenantIdFromRequest();
            var kullanicilar = await _kullaniciRepository.GetAllByTenantIdAsync(tenantId);
            return Ok(kullanicilar);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenantId = GetTenantIdFromRequest();
            var kullanici = await _kullaniciRepository.GetByIdAsync(id, tenantId);

            if (kullanici == null)
                return NotFound();

            return Ok(kullanici);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SiteYoneticisi")]
        public async Task<IActionResult> Create([FromBody] CreateKullaniciRequest request)
        {
            var tenantId = GetTenantIdFromRequest();

            if (await _kullaniciRepository.ExistsByEmailAsync(request.Email, tenantId))
                return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor" });

            // Şifreyi hash'le
            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            var kullanici = new Kullanici(
                tenantId,
                request.Ad,
                request.Soyad,
                request.Email,
                hashedPassword,
                request.TelefonNo,
                request.RolId);

            var result = await _kullaniciRepository.AddAsync(kullanici);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SiteYoneticisi")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateKullaniciRequest request)
        {
            var tenantId = GetTenantIdFromRequest();
            var kullanici = await _kullaniciRepository.GetByIdAsync(id, tenantId);

            if (kullanici == null)
                return NotFound();

            // Eğer email değiştiyse ve yeni email zaten kullanımdaysa
            if (kullanici.Email != request.Email && await _kullaniciRepository.ExistsByEmailAsync(request.Email, tenantId))
                return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor" });

            kullanici.UpdateKullaniciInfo(
                request.Ad,
                request.Soyad,
                request.Email,
                request.TelefonNo);

            kullanici.SetRol(request.RolId);

            if (request.Aktif.HasValue)
            {
                kullanici.SetAktif(request.Aktif.Value);
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                var hashedPassword = PasswordHasher.HashPassword(request.Password);
                kullanici.SetSifre(hashedPassword);
            }

            await _kullaniciRepository.UpdateAsync(kullanici);

            return Ok(kullanici);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SiteYoneticisi")]
        public async Task<IActionResult> Delete(int id)
        {
            var tenantId = GetTenantIdFromRequest();
            var kullanici = await _kullaniciRepository.GetByIdAsync(id, tenantId);

            if (kullanici == null)
                return NotFound();

            // Kendini silemez
            if (CurrentUser.UserId.HasValue && CurrentUser.UserId.Value == id)
                return BadRequest(new { message = "Kendinizi silemezsiniz" });

            await _kullaniciRepository.DeleteAsync(id, tenantId);

            return NoContent();
        }
    }

    public class CreateKullaniciRequest
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TelefonNo { get; set; }
        public int? RolId { get; set; }
    }

    public class UpdateKullaniciRequest
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Boş ise şifre değişmez
        public string TelefonNo { get; set; }
        public int? RolId { get; set; }
        public bool? Aktif { get; set; }
    }
}