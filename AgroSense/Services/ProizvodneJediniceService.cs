using AgroSense.DTOs.ProizvodnaJedinica;
using AgroSense.Repositories.ProizvodneJedinice;

namespace AgroSense.Services
{
    public class ProizvodneJediniceService
    {
        private readonly ProizvodneJediniceRepository _repo;

        public ProizvodneJediniceService(ProizvodneJediniceRepository repo)
        {
            _repo = repo;
        }

        public void Dodaj(ProizvodnaJedinicaCreateDto dto)
        {
            _repo.Dodaj(dto);
        }

        public List<ProizvodnaJedinicaResponseDto> VratiSve()
        {
            return _repo.VratiSve();
        }

        public ProizvodnaJedinicaViewDto? VratiPoId(Guid id)
        {
            return _repo.VratiPoId(id);
        }

        public void Update(Guid id, ProizvodnaJedinicaUpdateDto dto)
        {
            _repo.Update(id, dto);
        }
        public void Obrisi(Guid id)
        {
            _repo.Obrisi(id);
        }
        public List<Guid> VratiSveIdjeve()
        {
            return _repo.VratiSveIdjeve();
        }

    }
}
