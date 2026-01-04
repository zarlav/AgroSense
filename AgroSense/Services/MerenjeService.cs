using AgroSense.Domain;
using AgroSense.DTOs.Merenje;
using AgroSense.Repositories.Merenja;
using Cassandra;

namespace AgroSense.Services
{
    public class MerenjaService
    {
        private readonly MerenjaPoDanuRepository PoDanu;
        private readonly MerenjaPoLokacijiRepository PoLokaciji;
        private readonly MerenjaPoslednjaVrednostRepository PoslednjaVrednost;

        public MerenjaService(MerenjaPoDanuRepository poDanu,MerenjaPoLokacijiRepository poLokaciji, MerenjaPoslednjaVrednostRepository poslednjeMerenje)
        {
            PoDanu = poDanu;
            PoLokaciji = poLokaciji;
            PoslednjaVrednost = poslednjeMerenje;
        }

        public void SacuvajMerenje(MerenjeCreateDto m)
        {
            PoDanu.DodajMerenja(m);
            PoLokaciji.DodajMerenja(m);
            PoslednjaVrednost.DodajMerenja(m);
        }

        public List<MerenjeResponseDto> VratiMerenjePoDanu(Guid senzor_id, LocalDate dan)
        {
            return PoDanu.VratiMerenjaPoDanu(senzor_id, dan);
        }
        public List<DTOs.Merenje.MerenjeResponseDto> VratiPoslednjeMerenje(Guid senzor_id)
        {
            return PoslednjaVrednost.VratiPoslednjeMerenje(senzor_id);
        }
        public List<MerenjeResponseDto> VratiMerenjePoLokaciji(Guid lokacijaId, LocalDate dan)
        {
            return PoLokaciji.VratiMerenjaPoLokaciji(lokacijaId, dan);
        }
        public List<MerenjeResponseDto> VratiMerenjePoVremenu(Guid senzor_id, LocalDate dan, TimeSpan vremeOd, TimeSpan vremeDo)
        {
            return PoDanu.VratiMerenjaPoVremeneskomOpsegu(senzor_id, dan, vremeOd, vremeDo);
        }
    }
}
