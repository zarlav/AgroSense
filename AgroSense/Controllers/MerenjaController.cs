using AgroSense.Domain;
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
        public IActionResult DodajMerenje([FromBody] MerenjeCreateDto merenje)
        {
            if (merenje == null)
                return BadRequest("Merenje nije poslato.");

            try
            {
                _service.SacuvajMerenje(merenje);
                return Ok("Merenje uspesno dodato!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri dodavanju merenja: {ex.Message}");
            }
        }
        [HttpGet("po_vremenu")]
        public IActionResult VratiMerenjePoVremenu([FromQuery] Guid senzorId, [FromQuery] DateTime dan, [FromQuery] TimeSpan vremeOd, [FromQuery] TimeSpan vremeDo)
        {
            try
            {
                var datum = new Cassandra.LocalDate(dan.Year, dan.Month, dan.Day);
                var rezultat = _service.VratiMerenjePoVremenu(senzorId, datum, vremeOd, vremeDo);
                if (rezultat.Count == 0)
                    return NotFound("Nema izmerenih vrednosti za dati senzor.");
                return Ok(rezultat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju izmerenih vrednosti: {ex.Message}");
            }
        }

        [HttpGet("po_lokaciji")]
        public IActionResult VratiMerenjePoLokaciji([FromQuery] Guid lokacijaId, [FromQuery] DateTime dan)
        {
            try
            {
                var datum = new Cassandra.LocalDate(dan.Year, dan.Month, dan.Day);
                var rezultat = _service.VratiMerenjePoLokaciji(lokacijaId, datum);
                if (rezultat.Count == 0)
                    return NotFound("Nema izmerenih vrednosti za dati senzor na datoj lokaciji.");
                return Ok(rezultat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri vracanju izmerenih vrednosti: {ex.Message}");
            }
        }
        [HttpGet("poslednje_merenje")]
        public IActionResult VratiPoslednjeMerenje([FromQuery] Guid senzorId)
        {
            try
            {
                var rezultat = _service.VratiPoslednjeMerenje(senzorId);
                if (rezultat.Count == 0)
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
