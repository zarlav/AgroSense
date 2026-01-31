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
        public async Task<IActionResult> Dodaj([FromBody] ProizvodnaJedinicaCreateDto dto)
        {
            await _service.Dodaj(dto);
            return Ok("Proizvodna jedinica dodata.");
        }

        [HttpGet]
        public async Task<IActionResult> VratiSve()
        {
            return Ok(await _service.VratiSve());
        }

        [HttpGet("{tip}/{id}")]
        public async Task<IActionResult> VratiJednu(string tip,bool stanje, Guid id)
        {
            var jedinica = await _service.VratiPoId(tip, stanje, id);
            if (jedinica == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            return Ok(jedinica);
        }

        [HttpPut("{tip}/{id}")]
        public async Task<IActionResult> Update(string tip,bool stanje, Guid id, [FromBody] ProizvodnaJedinicaUpdateDto dto)
        {
            var postojeca = await _service.VratiPoId(tip, stanje, id);
            if (postojeca == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            await _service.Update(tip,stanje, id, dto);
            return Ok("Proizvodna jedinica je uspesno azurirana.");
        }

        [HttpDelete("{tip}/{id}")]
        public async Task<IActionResult> Obrisi(string tip,bool stanje, Guid id)
        {
            var jedinica = await _service.VratiPoId(tip,stanje, id);
            if (jedinica == null)
                return NotFound("Proizvodna jedinica ne postoji.");

            await _service.Obrisi(tip,stanje, id);
            return Ok("Proizvodna jedinica je deaktivirana.");
        }

        [HttpGet("ids")]
        public async Task<IActionResult> VratiSveIdjeve()
        {
            return Ok(await _service.VratiSveIdjeve());
        }
    }
}