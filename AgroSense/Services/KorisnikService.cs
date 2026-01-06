using AgroSense.DTOs.Korisnik;
using AgroSense.Repositories.Korisnici;

namespace AgroSense.Services
{
    public class KorisnikService
    {
        private readonly KorisnikRepository _repo;

        public KorisnikService(KorisnikRepository repo)
        {
            _repo = repo;
        }

        public void Dodaj(KorisnikCreateDto dto) => _repo.Dodaj(dto);

        public List<KorisnikResponseDto> VratiSve() => _repo.VratiSve();

        public KorisnikViewDto? VratiPoId(Guid id) => _repo.VratiPoId(id);

        public void Update(Guid id, KorisnikCreateDto dto) => _repo.Update(id, dto);

        public void Delete(Guid id) => _repo.Delete(id);
    }
}
