using AgroSense.DTOs.Korisnik;
using AgroSense.Repositories.Korisnici;
using System.Threading.Tasks;

namespace AgroSense.Services
{
    public class KorisnikService
    {
        private readonly KorisnikRepository _repo;

        public KorisnikService(KorisnikRepository repo)
        {
            _repo = repo;
        }

        public async Task Dodaj(KorisnikCreateDto dto)
        {
            await _repo.Dodaj(dto);
        }

        public async Task<List<KorisnikResponseDto>> VratiSve()
        {
            return await _repo.VratiSve();
        }

        public async Task<KorisnikViewDto?> VratiPoId(Guid id)
        {
            return await  _repo.VratiPoId(id);
        }

        public async Task Update(Guid id, KorisnikCreateDto dto)
        {
            await _repo.Update(id, dto);
        }

        public async Task Delete(Guid id)
        {
           await _repo.Delete(id);
        }
    }
}
