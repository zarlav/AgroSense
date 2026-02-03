using AgroSense.DTOs.Merenje;
using AgroSense.Repositories.Merenja;
using AgroSense.Services;
using Cassandra;
using Microsoft.AspNetCore.Mvc;

namespace AgroSense.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerenjaController : ControllerBase
    {
        private readonly MerenjaService _service;

        public MerenjaController(MerenjaService service)
        {
            _service = service;
        }
        [HttpPost]
        public async  Task<IActionResult> DodajMerenje([FromBody] MerenjeCreateDto merenje)
        {
            if (merenje == null)
                return BadRequest("Merenje nije poslato.");

            try
            {
                await _service.SacuvajMerenje(merenje);
                return Ok("Merenje uspesno dodato!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri dodavanju merenja: {ex.Message}");
            }
        }
        [HttpGet("po_vremenu")]
        public async Task<IActionResult> VratiMerenjePoVremenu([FromQuery] Guid senzorId, [FromQuery] DateTime dan, [FromQuery] TimeSpan vremeOd, [FromQuery] TimeSpan vremeDo)
        {
            try
            {
                var datum = new Cassandra.LocalDate(dan.Year, dan.Month, dan.Day);
                var rezultat = await _service.VratiMerenjePoVremenu(senzorId, datum, vremeOd, vremeDo);
                if (rezultat == null || rezultat.Count == 0)
                    return NotFound("Nema izmerenih vrednosti za dati senzor.");
                return Ok(rezultat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju izmerenih vrednosti: {ex.Message}");
            }
        }

        [HttpGet("po_lokaciji")]
        public async Task<IActionResult> VratiMerenjePoLokaciji([FromQuery] Guid lokacijaId, [FromQuery] DateTime dan, [FromQuery] TimeSpan vremeOd, [FromQuery] TimeSpan vremeDo)
        {
            try
            {
                var datum = new Cassandra.LocalDate(dan.Year, dan.Month, dan.Day);
                var rezultat = await _service.VratiMerenjePoLokaciji(lokacijaId, datum, vremeOd, vremeDo);
                if (rezultat == null || rezultat.Count == 0)
                    return NotFound("Nema izmerenih vrednosti za dati senzor na datoj lokaciji.");
                return Ok(rezultat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju izmerenih vrednosti: {ex.Message}");
            }
        }
        [HttpGet("poslednje_merenje")]
        public async Task<IActionResult> VratiPoslednjeMerenje([FromQuery] Guid senzorId)
        {
            try
            {
                var rezultat = await _service.VratiPoslednjeMerenje(senzorId);
                if (rezultat == null)
                    return NotFound("Nema izmerenih vrednosti za dati senzor!");
                return Ok(rezultat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju izmerenih vrednosti: {ex.Message}");
            }
        }
    }
}
