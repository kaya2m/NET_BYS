using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Domain.TenantYonetimi.Entities;
using NET.Domain.TenantYonetimi.Interfaces;

namespace NET.API.Controllers.TenantYonetimi
{
    [Authorize(Roles ="Admin")]
    public class RolController : ApiControllerBase
    {
        private readonly IRolRepository _rolRepository;

        public RolController(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _rolRepository.GetAllByTenantIdAsync(CurrentUser.TenantId);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenantId = GetTenantIdFromRequest();
            var role = await _rolRepository.GetByIdAsync(id, tenantId);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet("{id}/izinler")]
        public async Task<IActionResult> GetIzinlerByRolId(int id)
        {
            if (!await _rolRepository.ExistsByIdAsync(id, CurrentUser.TenantId))
                return NotFound();

            var izinler = await _rolRepository.GetIzinlerByRolIdAsync(id, CurrentUser.TenantId);
            return Ok(izinler);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRolRequest request)
        {
            var tenantId = GetTenantIdFromRequest();

            var rol = new Rol(
                tenantId,
                request.Ad,
                request.Aciklama);

            var result = await _rolRepository.AddAsync(rol);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRolRequest request)
        {
            var tenantId = GetTenantIdFromRequest();
            var rol = await _rolRepository.GetByIdAsync(id, tenantId);

            if (rol == null)
                return NotFound();

            rol.UpdateRolInfo(
                request.Ad,
                request.Aciklama);

            await _rolRepository.UpdateAsync(rol);

            return Ok(rol);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tenantId = GetTenantIdFromRequest();
            var rol = await _rolRepository.GetByIdAsync(id, tenantId);

            if (rol == null)
                return NotFound();

            await _rolRepository.DeleteAsync(id, tenantId);

            return NoContent();
        }

        [HttpPost("{rolId}/izinler")]
        public async Task<IActionResult> AddIzinToRol(int rolId, [FromBody] AddIzinToRolRequest request)
        {
            var tenantId = GetTenantIdFromRequest();

            if (!await _rolRepository.ExistsByIdAsync(rolId, tenantId))
                return NotFound(new { message = "Rol bulunamadı" });

            var rolIzin = new RolIzin(tenantId, rolId, request.IzinId);

            var result = await _rolRepository.AddRolIzinAsync(rolIzin);

            if (!result)
                return BadRequest(new { message = "İzin eklenemedi" });

            return Ok(new { message = "İzin başarıyla eklendi" });
        }

        [HttpDelete("{rolId}/izinler/{izinId}")]
        public async Task<IActionResult> RemoveIzinFromRol(int rolId, int izinId)
        {
            var tenantId = GetTenantIdFromRequest();

            if (!await _rolRepository.ExistsByIdAsync(rolId, tenantId))
                return NotFound(new { message = "Rol bulunamadı" });

            var result = await _rolRepository.RemoveRolIzinAsync(rolId, izinId, tenantId);

            if (!result)
                return BadRequest(new { message = "İzin kaldırılamadı" });

            return Ok(new { message = "İzin başarıyla kaldırıldı" });
        }
    }

    public class CreateRolRequest
    {
        public string Ad { get; set; }
        public string Aciklama { get; set; }
    }

    public class UpdateRolRequest
    {
        public string Ad { get; set; }
        public string Aciklama { get; set; }
    }

    public class AddIzinToRolRequest
    {
        public int IzinId { get; set; }
    }
}