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

        public async Task SacuvajMerenje(MerenjeCreateDto m)
        {
            await PoDanu.DodajMerenja(m);
            await PoLokaciji.DodajMerenja(m);
            await PoslednjaVrednost.DodajMerenja(m);
        }

        public async Task<List<MerenjeResponseDto>> VratiMerenjePoDanu(Guid senzor_id, LocalDate dan)
        {
            return await PoDanu.VratiMerenjaPoDanu(senzor_id, dan);
        }
        public async Task<DTOs.Merenje.MerenjeResponseDto?> VratiPoslednjeMerenje(Guid senzor_id)
        {
            return await PoslednjaVrednost.VratiPoslednjeMerenje(senzor_id);
        }
        public async Task<List<MerenjeResponseDto>?> VratiMerenjePoLokaciji(Guid lokacijaId, LocalDate dan, TimeSpan vremeOd, TimeSpan vremeDo)
        {
            return await PoLokaciji.VratiMerenjaPoLokaciji(lokacijaId, dan, vremeOd, vremeDo);
        }
        public async Task<List<MerenjeResponseDto>> VratiMerenjePoVremenu(Guid senzor_id, LocalDate dan, TimeSpan vremeOd, TimeSpan vremeDo)
        {
            return await PoDanu.VratiMerenjaPoVremeneskomOpsegu(senzor_id, dan, vremeOd, vremeDo);
        }
    }
}
