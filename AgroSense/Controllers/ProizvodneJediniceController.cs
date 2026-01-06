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

        [HttpGet("{id}")]
        public IActionResult VratiJednu(Guid id)
        {
            var jedinica = _service.VratiPoId(id);
            if (jedinica == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            return Ok(jedinica);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] ProizvodnaJedinicaUpdateDto dto)
        {
            var postojeca = _service.VratiPoId(id);
            if (postojeca == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            _service.Update(id, dto);
            return Ok("Proizvodna jedinica uspešno ažurirana.");
        }

        [HttpDelete("{id}")]
        public IActionResult Obrisi(Guid id)
        {
            var jedinica = _service.VratiPoId(id);
            if (jedinica == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            _service.Obrisi(id);
            return Ok("Proizvodna jedinica je deaktivirana.");
        }
        [HttpGet("ids")]
        public IActionResult VratiSveIdjeve()
        {
            return Ok(_service.VratiSveIdjeve());
        }

    }
}
