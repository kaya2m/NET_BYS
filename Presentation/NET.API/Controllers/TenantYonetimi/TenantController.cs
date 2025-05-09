using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Domain.TenantYonetimi.Entities;
using NET.Domain.TenantYonetimi.Interfaces;

namespace NET.API.Controllers.TenantYonetimi
{
    [Authorize(Roles = "Admin")]
    public class TenantController : ApiControllerBase
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);

            if (tenant == null)
                return NotFound();

            return Ok(tenant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantRequest request)
        {
            if (await _tenantRepository.ExistsByHostAsync(request.Host))
                return BadRequest(new { message = "Bu host adı zaten kullanılıyor" });

            var tenant = new Tenant(
                request.Ad,
                request.Host,
                request.Aciklama,
                request.Logo);

            if (request.MaxKullaniciSayisi > 0 || request.MaxSiteSayisi > 0)
            {
                tenant.SetLimits(
                    request.MaxKullaniciSayisi > 0 ? request.MaxKullaniciSayisi : 10,
                    request.MaxSiteSayisi > 0 ? request.MaxSiteSayisi : 1,
                    request.MaxDaireSayisi);
            }

            var result = await _tenantRepository.AddAsync(tenant);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTenantRequest request)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);

            if (tenant == null)
                return NotFound();

            // Eğer host değiştiyse ve yeni host zaten kullanımdaysa
            if (tenant.Host != request.Host && await _tenantRepository.ExistsByHostAsync(request.Host))
                return BadRequest(new { message = "Bu host adı zaten kullanılıyor" });

            tenant.UpdateTenantInfo(
                request.Ad,
                request.Host,
                request.Aciklama,
                request.Logo);

            if (request.MaxKullaniciSayisi > 0 || request.MaxSiteSayisi > 0)
            {
                tenant.SetLimits(
                    request.MaxKullaniciSayisi,
                    request.MaxSiteSayisi,
                    request.MaxDaireSayisi);
            }

            if (request.Aktif.HasValue)
            {
                tenant.SetAktif(request.Aktif.Value);
            }

            await _tenantRepository.UpdateAsync(tenant);

            return Ok(tenant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);

            if (tenant == null)
                return NotFound();

            await _tenantRepository.DeleteAsync(id);

            return NoContent();
        }
    }

    public class CreateTenantRequest
    {
        public string Ad { get; set; }
        public string Host { get; set; }
        public string Aciklama { get; set; }
        public string Logo { get; set; }
        public int MaxKullaniciSayisi { get; set; } = 10;
        public int MaxSiteSayisi { get; set; } = 1;
        public int? MaxDaireSayisi { get; set; }
    }

    public class UpdateTenantRequest
    {
        public string Ad { get; set; }
        public string Host { get; set; }
        public string Aciklama { get; set; }
        public string Logo { get; set; }
        public int MaxKullaniciSayisi { get; set; }
        public int MaxSiteSayisi { get; set; }
        public int? MaxDaireSayisi { get; set; }
        public bool? Aktif { get; set; }
    }
}