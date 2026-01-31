using AgroSense.DTOs.Senzor;
using AgroSense.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgroSense.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SenzorController : ControllerBase
    {
        private readonly Services.SenzorService _service;
        public SenzorController(Services.SenzorService service)
        {
            _service = service;
        }
        [HttpPost("DodajSenzor")]
        public async Task<IActionResult> DodajSenzor([FromBody] SenzorCreateDto senzor)
        {
            if (senzor == null)
                return BadRequest("Senzor nije dodat!");
            try
            {
                await _service.DodajSenzor(senzor);
                return Ok("Senzor je uspesno dodat!");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Greska pri dodavanju senzora! : {e.Message}");
            }
        }
        [HttpGet("senzor")]
        public async Task<IActionResult> VratiSenzor([FromQuery] Guid senzorId)
        {
            try
            {
                var senzor = await _service.VratiSenzor(senzorId);
                return Ok(senzor);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju senzora: {ex.Message}");
            }
        }
        [HttpGet("svi_senzori")]
        public async Task<IActionResult> VratiSveSenzore()
        {
            try
            {
                var senzori = await _service.VratiSveSenzore();
                return Ok(senzori);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju svih senzora: {ex.Message}");
            }
        }
        [HttpGet("svi_senzori_ids")]
        public async Task<IActionResult> VratiSveIdSenzora()
        {
            var ids = await _service.VratiSveIdSenzora();

            if (ids.Count == 0)
                return NoContent();

            return Ok(ids);
        }
    }
}
