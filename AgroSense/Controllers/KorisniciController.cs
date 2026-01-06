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
        public IActionResult VratiSve()
        {
            return Ok(_service.VratiSve());
        }

        [HttpGet("{id}")]
        public IActionResult VratiJednog(Guid id)
        {
            var korisnik = _service.VratiPoId(id);
            if (korisnik == null)
                return NotFound();

            return Ok(korisnik);
        }
    }
}
