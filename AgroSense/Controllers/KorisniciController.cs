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
        public IActionResult Dodaj([FromBody] KorisnikCreateDto dto)
        {
            _service.Dodaj(dto);
            return Ok("Korisnik dodat.");
        }

        [HttpGet]
        public IActionResult VratiSve() => Ok(_service.VratiSve());

        [HttpGet("{id}")]
        public IActionResult VratiJednog(Guid id)
        {
            var korisnik = _service.VratiPoId(id);
            if (korisnik == null) return NotFound("Korisnik ne postoji.");
            return Ok(korisnik);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] KorisnikCreateDto dto)
        {
            _service.Update(id, dto);
            return Ok("Korisnik ažuriran.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return Ok("Korisnik obrisan.");
        }
    }
}
