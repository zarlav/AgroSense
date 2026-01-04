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
        public IActionResult DodajSenzor([FromBody] SenzorCreateDto senzor)
        {
            if (senzor == null)
                return BadRequest("Senzor nije dodat!");
            try
            {
                _service.DodajSenzor(senzor);
                return Ok("Senzor je uspesno dodat!");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Greska pri dodavanju senzora! : {e.Message}");
            }
        }
        [HttpGet("senzor")]
        public IActionResult VratiSenzor([FromQuery] Guid senzorId)
        {
            try
            {
                var senzor = _service.VratiSenzor(senzorId);
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
        public IActionResult VratiSveSenzore()
        {
            try
            {
                var senzori = _service.VratiSveSenzore();
                return Ok(senzori);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju svih senzora: {ex.Message}");
            }
        }
        [HttpGet("svi_senzori_ids")]
        public IActionResult VratiSveIdSenzora()
        {
            var ids = _service.VratiSveIdSenzora();

            if (ids.Count == 0)
                return NoContent();

            return Ok(ids);
        }
    }
}
