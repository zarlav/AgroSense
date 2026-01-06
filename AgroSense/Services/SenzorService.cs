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
        public void DodajSenzor(SenzorCreateDto s)
        {
            senzor.DodajSenzor(s);
        }

        public SenzorResponseDto VratiSenzor(Guid senzorId)
        {
            var s = senzor.VratiSenzor(senzorId);

            if (s == null)
                throw new KeyNotFoundException("Senzor ne postoji");

            return s;
        }

        public List<SenzorIdDto> VratiSveIdSenzora()
        {
            return senzor.VratiSveIdSenzora(); 
        }

        public List<SenzorResponseDto> VratiSveSenzore()
        {
            return senzor.VratiSveSenzore();
        }
    }
}
