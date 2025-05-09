// NET.API/Controllers/SiteYonetimi/SiteController.cs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Domain.SiteYonetimi.Entities;
using NET.Domain.SiteYonetimi.Interfaces;

namespace NET.API.Controllers.SiteYonetimi
{
    [Authorize]
    public class SiteController : ApiControllerBase
    {
        private readonly ISiteRepository _siteRepository;

        public SiteController(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenantId = GetTenantIdFromRequest();
            var sites = await _siteRepository.GetAllByTenantIdAsync(tenantId);
            return Ok(sites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenantId = GetTenantIdFromRequest();
            var site = await _siteRepository.GetByIdAsync(id, tenantId);

            if (site == null)
                return NotFound();

            return Ok(site);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SiteYoneticisi")]
        public async Task<IActionResult> Create([FromBody] CreateSiteRequest request)
        {
            var tenantId = GetTenantIdFromRequest();

            var site = new Site(
                tenantId,
                request.Ad,
                request.Il,
                request.Ilce,
                request.Mahalle,
                request.No,
                request.YoneticiAdi,
                request.YoneticiTelefon,
                request.YoneticiEmail,
                request.Aciklama);

            site.SetAdres(request.Sokak, request.Cadde, request.PostaKodu);

            if (request.Enlem.HasValue && request.Boylam.HasValue)
            {
                site.SetKoordinatlar(request.Enlem.Value, request.Boylam.Value);
            }

            var result = await _siteRepository.AddAsync(site);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SiteYoneticisi")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSiteRequest request)
        {
            var tenantId = GetTenantIdFromRequest();
            var site = await _siteRepository.GetByIdAsync(id, tenantId);

            if (site == null)
                return NotFound();

            site.UpdateSiteInfo(
                request.Ad,
                request.Il,
                request.Ilce,
                request.Mahalle,
                request.No,
                request.YoneticiAdi,
                request.YoneticiTelefon,
                request.YoneticiEmail,
                request.Aciklama);

            site.SetAdres(request.Sokak, request.Cadde, request.PostaKodu);

            if (request.Enlem.HasValue && request.Boylam.HasValue)
            {
                site.SetKoordinatlar(request.Enlem.Value, request.Boylam.Value);
            }

            if (request.Aktif.HasValue)
            {
                site.SetAktif(request.Aktif.Value);
            }

            await _siteRepository.UpdateAsync(site);

            return Ok(site);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var tenantId = GetTenantIdFromRequest();
            var site = await _siteRepository.GetByIdAsync(id, tenantId);

            if (site == null)
                return NotFound();

            await _siteRepository.DeleteAsync(id, tenantId);

            return NoContent();
        }
    }

    public class CreateSiteRequest
    {
        public string Ad { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Mahalle { get; set; }
        public string Sokak { get; set; }
        public string Cadde { get; set; }
        public string No { get; set; }
        public string PostaKodu { get; set; }
        public decimal? Enlem { get; set; }
        public decimal? Boylam { get; set; }
        public string YoneticiAdi { get; set; }
        public string YoneticiTelefon { get; set; }
        public string YoneticiEmail { get; set; }
        public string Aciklama { get; set; }
    }

    public class UpdateSiteRequest
    {
        public string Ad { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Mahalle { get; set; }
        public string Sokak { get; set; }
        public string Cadde { get; set; }
        public string No { get; set; }
        public string PostaKodu { get; set; }
        public decimal? Enlem { get; set; }
        public decimal? Boylam { get; set; }
        public string YoneticiAdi { get; set; }
        public string YoneticiTelefon { get; set; }
        public string YoneticiEmail { get; set; }
        public string Aciklama { get; set; }
        public bool? Aktif { get; set; }
    }
}