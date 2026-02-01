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
        private readonly AlarmService AlarmSer;

        public MerenjaService(MerenjaPoDanuRepository poDanu,MerenjaPoLokacijiRepository poLokaciji, MerenjaPoslednjaVrednostRepository poslednjeMerenje, AlarmService AlarmSer)
        {
            PoDanu = poDanu;
            PoLokaciji = poLokaciji;
            PoslednjaVrednost = poslednjeMerenje;
            this.AlarmSer = AlarmSer;
            if (AlarmSer == null)
                throw new Exception("AlarmSer je null");
        }

        public async Task SacuvajMerenje(MerenjeCreateDto m)
        {
            try
            {
                if (m.Id_lokacije == Guid.Empty)
                {
                    throw new Exception("Id_lokacije je prazan!");
                }
                if (PoDanu == null) throw new Exception("PoDanu je null!");
                if (PoLokaciji == null) throw new Exception("PoLokaciji je null!");
                if (PoslednjaVrednost == null) throw new Exception("PoslednjaVrednost je null!");
                if (AlarmSer == null) throw new Exception("AlarmSer je null!");
                Console.WriteLine("Pre PoDanu");
                await PoDanu.DodajMerenja(m);
                Console.WriteLine("Pre PoLokaciji");
                await PoLokaciji.DodajMerenja(m);
                Console.WriteLine("Pre PoslednjaVrednost");
                await PoslednjaVrednost.DodajMerenja(m);
                Console.WriteLine("Pre AlarmSer");
                await AlarmSer.ProveriAlarm(m.Id_lokacije, true);
                Console.WriteLine("Sve proslo");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
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
