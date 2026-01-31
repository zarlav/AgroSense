using AgroSense.Domain;
using AgroSense.DTOs.Senzor;
using AgroSense.Repositories.Senzor;

namespace AgroSense.Services
{
    public class SenzorService
    {
        private readonly SenzorRepository senzor;
        public SenzorService(SenzorRepository _senzor)
        {
            senzor = _senzor;
        }
        public async Task DodajSenzor(SenzorCreateDto s)
        {
            await senzor.DodajSenzor(s);
        }

        public async Task<SenzorResponseDto?> VratiSenzor(Guid senzorId)
        {
            var s = senzor.VratiSenzor(senzorId);

            if (s == null)
                throw new KeyNotFoundException("Senzor ne postoji");

            return await s;
        }

        public async Task<List<SenzorIdDto>> VratiSveIdSenzora()
        {
            return await senzor.VratiSveIdSenzora(); 
        }

        public async Task<List<SenzorResponseDto>> VratiSveSenzore()
        {
            return  await senzor.VratiSveSenzore();
        }
    }
}
