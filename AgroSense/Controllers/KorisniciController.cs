using AgroSense.DTOs.Korisnik;
using AgroSense.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgroSense.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KorisniciController : ControllerBase
    {
        private readonly KorisnikService _service;

        public KorisniciController(KorisnikService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task <IActionResult> Dodaj([FromBody] KorisnikCreateDto dto)
        {
            await _service.Dodaj(dto);
            return Ok("Korisnik dodat.");
        }

        [HttpGet]
        public async Task<IActionResult> VratiSve()
        {
           return Ok(await _service.VratiSve());    
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> VratiJednog(Guid id)
        {
            var korisnik = await _service.VratiPoId(id);
            if (korisnik == null) return NotFound("Korisnik ne postoji.");
            return Ok(korisnik);
        }

        [HttpPut("{id}")]
        public async Task <IActionResult> Update(Guid id, [FromBody] KorisnikCreateDto dto)
        {
            await _service.Update(id, dto);
            return Ok("Korisnik ažuriran.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return Ok("Korisnik obrisan.");
        }
    }
}
