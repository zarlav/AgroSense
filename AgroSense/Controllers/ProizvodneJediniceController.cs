using AgroSense.DTOs.ProizvodnaJedinica;
using AgroSense.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgroSense.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProizvodneJediniceController : ControllerBase
    {
        private readonly ProizvodneJediniceService _service;

        public ProizvodneJediniceController(ProizvodneJediniceService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Dodaj([FromBody] ProizvodnaJedinicaCreateDto dto)
        {
            _service.Dodaj(dto);
            return Ok("Proizvodna jedinica dodata.");
        }

        [HttpGet]
        public IActionResult VratiSve()
        {
            return Ok(_service.VratiSve());
        }

        // Potrebna su oba ključa zbog strukture tabele
        [HttpGet("{tip}/{id}")]
        public IActionResult VratiJednu(string tip, Guid id)
        {
            var jedinica = _service.VratiPoId(tip, id);
            if (jedinica == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            return Ok(jedinica);
        }

        [HttpPut("{tip}/{id}")]
        public IActionResult Update(string tip, Guid id, [FromBody] ProizvodnaJedinicaUpdateDto dto)
        {
            var postojeca = _service.VratiPoId(tip, id);
            if (postojeca == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            _service.Update(tip, id, dto);
            return Ok("Proizvodna jedinica uspešno ažurirana.");
        }

        [HttpDelete("{tip}/{id}")]
        public IActionResult Obrisi(string tip, Guid id)
        {
            var jedinica = _service.VratiPoId(tip, id);
            if (jedinica == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            _service.Obrisi(tip, id);
            return Ok("Proizvodna jedinica je deaktivirana.");
        }

        [HttpGet("ids")]
        public IActionResult VratiSveIdjeve()
        {
            return Ok(_service.VratiSveIdjeve());
        }
    }
}