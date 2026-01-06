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

        public void Dodaj(KorisnikCreateDto dto)
        {
            _repo.Dodaj(dto);
        }

        public List<KorisnikResponseDto> VratiSve()
        {
            return _repo.VratiSve();
        }

        public KorisnikViewDto? VratiPoId(Guid id)
        {
            return _repo.VratiPoId(id);
        }
    }
}
