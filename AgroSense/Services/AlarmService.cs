using AgroSense.DTOs.Alarm;
using AgroSense.Repositories.Merenja;
using AgroSense.Repositories.ProizvodneJedinice;
using Cassandra;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading.Tasks;

namespace AgroSense.Services
{
    public class AlarmService
    {
        private readonly AlarmRepository _repository;
        private readonly ProizvodneJediniceRepository _repositoryProizvodnaJedinica;
        private readonly MerenjaPoLokacijiRepository _repositoryPoLokaciji;

        public AlarmService(AlarmRepository repository, ProizvodneJediniceRepository repositoryProizvodnaJedinica, MerenjaPoLokacijiRepository repositoryPoLokaciji)
        {
            _repository = repository;
            _repositoryProizvodnaJedinica = repositoryProizvodnaJedinica;
            _repositoryPoLokaciji = repositoryPoLokaciji;
        }

        public async Task<List<AlarmResponseDTO>> VratiAlarmePoJedinici(
            Guid idJedinice,
            LocalDate dan,
            TimeSpan vremeOd,
            TimeSpan vremeDo)
        {
            return await _repository.VratiAlarmePoJedinici(idJedinice, dan, vremeOd, vremeDo);
        }

        public async Task ProveriAlarm(Guid idJedinice, bool stanje)
        {
            DateTime sada = DateTime.UtcNow;
            var localDate = new Cassandra.LocalDate(sada.Year, sada.Month, sada.Day);
            var vremeTrenutno = sada.TimeOfDay;
            var vremePreMinuta = vremeTrenutno - TimeSpan.FromMinutes(1);
            vremeTrenutno += TimeSpan.FromMinutes(1);

            var tip = await _repositoryProizvodnaJedinica.VratiTipAktivneJedinice(idJedinice);
            if (tip == null)
                throw new InvalidOperationException($"Ne postoji aktivna proizvodna jedinica sa ID = {idJedinice}");
            var rezPJ = await _repositoryProizvodnaJedinica.VratiPoId(tip.ToString(), stanje, idJedinice);

            var rezMPL = await _repositoryPoLokaciji.VratiMerenjaPoLokaciji(
                idJedinice,
                localDate,
                vremePreMinuta,
                vremeTrenutno
            );
            var poslednjeMerenje = rezMPL.LastOrDefault();
            if(poslednjeMerenje != null && rezPJ != null)
            {
                var temp = poslednjeMerenje.Temperatura;
                var co2 = poslednjeMerenje.Co2;
                var vlaznostLista = poslednjeMerenje.Vlaznost_lista;
                var vlaznost = poslednjeMerenje.Vlaznost;
                var pritisakVazduha = poslednjeMerenje.Pritisak_vazduha;
                var pritisakUCevima = poslednjeMerenje.Pritisak_u_cevima;
                var uv = poslednjeMerenje.Uv_vrednost;
                Console.WriteLine($"temperatura {temp}");
                Console.WriteLine($"co2 {co2}");
                Console.WriteLine($"vlaznostLista {vlaznostLista}");
                Console.WriteLine($"vlaznost {vlaznost}");
                Console.WriteLine($"pritisakVazduha {pritisakVazduha}");
                Console.WriteLine($"pritisakUCevima  {pritisakUCevima}");
                Console.WriteLine($"uv {uv}");
                Console.WriteLine($"temp min{rezPJ.Granica_temp_min} - temp max {rezPJ.Granica_temp_max}");
                Console.WriteLine($"co2 min{rezPJ.Granica_co2_min} co2 max {rezPJ.Granica_co2_max}");
                Console.WriteLine($"vlaznost min{rezPJ.Granica_vlaznost_min} - vlaznost max {rezPJ.Granica_vlaznost_max}");
                Console.WriteLine($"vlaznost lista min{rezPJ.Granica_vlaznost_lista_min} - vlaznost lista max {rezPJ.Granica_vlaznost_lista_max}");
                Console.WriteLine($"pritisakVazduha min{rezPJ.Granica_pritisak_vazduha_min} - pritisakVazduha max {rezPJ.Granica_pritisak_vazduha_max}");
                Console.WriteLine($"pritisak  cevi min{rezPJ.Granica_pritisak_u_cevima_min} - pritisak u cevima max {rezPJ.Granica_pritisak_u_cevima_max}");
                var tasks = new List<Task>();

                if ((float)temp < (float)rezPJ.Granica_temp_min || (float)temp > (float)rezPJ.Granica_temp_max)
                {
                    Console.WriteLine("Temperatura prolaz");
                    var acdto = new AlarmCreateDTO
                    {
                        Id_jedinice = idJedinice,
                        Dan = new LocalDate(poslednjeMerenje.Datum.Year, poslednjeMerenje.Datum.Month, poslednjeMerenje.Datum.Day),
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Parametar = "Temperatura[C]", 
                        Trenutna_vrednost = temp,
                        Granicna_vrednostMin = rezPJ.Granica_temp_min,
                        Granicna_vrednostMax = rezPJ.Granica_temp_max,
                        Komentar = $"Temperatura je van dozvoljenog opsega ({rezPJ.Granica_temp_min}-{rezPJ.Granica_temp_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));

                }
                if(co2 < rezPJ.Granica_co2_min || co2 > rezPJ.Granica_co2_max)
                {
                    var acdto = new AlarmCreateDTO
                    {
                        Id_jedinice = idJedinice,
                        Dan = new LocalDate(poslednjeMerenje.Datum.Year, poslednjeMerenje.Datum.Month, poslednjeMerenje.Datum.Day),
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Parametar = "CO2[ppm]",
                        Trenutna_vrednost = co2,
                        Granicna_vrednostMin = rezPJ.Granica_co2_min,
                        Granicna_vrednostMax = rezPJ.Granica_co2_max,
                        Komentar = $"CO2 je van dozvoljenog opsega ({rezPJ.Granica_co2_min}-{rezPJ.Granica_co2_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));
                }
                if(vlaznostLista < rezPJ.Granica_vlaznost_lista_min || vlaznostLista > rezPJ.Granica_vlaznost_lista_max)
                {
                    var acdto = new AlarmCreateDTO
                    {
                        Id_jedinice = idJedinice,
                        Dan = new LocalDate(poslednjeMerenje.Datum.Year, poslednjeMerenje.Datum.Month, poslednjeMerenje.Datum.Day),
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Parametar = "Vlaznost lista[%]",
                        Trenutna_vrednost = vlaznostLista,
                        Granicna_vrednostMin = rezPJ.Granica_vlaznost_lista_min,
                        Granicna_vrednostMax = rezPJ.Granica_vlaznost_lista_max,
                        Komentar = $"Vlaznost lista je van dozvoljenog opsega ({rezPJ.Granica_vlaznost_lista_min}-{rezPJ.Granica_vlaznost_lista_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));

                }
                if(vlaznost < rezPJ.Granica_vlaznost_min || vlaznost > rezPJ.Granica_vlaznost_max)
                {
                    var acdto = new AlarmCreateDTO
                    {
                        Id_jedinice = idJedinice,
                        Dan = new LocalDate(poslednjeMerenje.Datum.Year, poslednjeMerenje.Datum.Month, poslednjeMerenje.Datum.Day),
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Parametar = "Vlaznost vazduha[%]",
                        Trenutna_vrednost = vlaznost,
                        Granicna_vrednostMin = rezPJ.Granica_vlaznost_min,
                        Granicna_vrednostMax = rezPJ.Granica_vlaznost_max,
                        Komentar = $"Vlaznost vazduha je van dozvoljenog opsega ({rezPJ.Granica_vlaznost_min}-{rezPJ.Granica_vlaznost_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));
                }
                if (pritisakVazduha < rezPJ.Granica_pritisak_vazduha_min || pritisakVazduha > rezPJ.Granica_pritisak_vazduha_max)
                {
                    var acdto = new AlarmCreateDTO
                    {
                        Id_jedinice = idJedinice,
                        Dan = new LocalDate(poslednjeMerenje.Datum.Year, poslednjeMerenje.Datum.Month, poslednjeMerenje.Datum.Day),
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Parametar = "Pritisak vazduha[hPa]",
                        Trenutna_vrednost = pritisakVazduha,
                        Granicna_vrednostMin = rezPJ.Granica_pritisak_vazduha_min,
                        Granicna_vrednostMax = rezPJ.Granica_pritisak_vazduha_max,
                        Komentar = $"Pritisak vazduha je van dozvoljenog opsega ({rezPJ.Granica_pritisak_vazduha_min}-{rezPJ.Granica_pritisak_vazduha_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));
                }
                if (pritisakUCevima < rezPJ.Granica_pritisak_u_cevima_min || pritisakUCevima > rezPJ.Granica_pritisak_u_cevima_max)
                {
                    var acdto = new AlarmCreateDTO
                    {
                        Id_jedinice = idJedinice,
                        Dan = new LocalDate(poslednjeMerenje.Datum.Year, poslednjeMerenje.Datum.Month, poslednjeMerenje.Datum.Day),
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Parametar = "Pritisak u cevima[bar]",
                        Trenutna_vrednost = pritisakUCevima,
                        Granicna_vrednostMin = rezPJ.Granica_pritisak_u_cevima_min,
                        Granicna_vrednostMax = rezPJ.Granica_pritisak_u_cevima_max,
                        Komentar = $"Pritisak u cevima je van dozvoljenog opsega ({rezPJ.Granica_pritisak_u_cevima_min}-{rezPJ.Granica_pritisak_u_cevima_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));
                }
                if (uv < rezPJ.Granica_svetlost_min || uv > rezPJ.Granica_svetlost_max)
                {
                    var acdto = new AlarmCreateDTO
                    {
                        Id_jedinice = idJedinice,
                        Dan = new LocalDate(poslednjeMerenje.Datum.Year, poslednjeMerenje.Datum.Month, poslednjeMerenje.Datum.Day),
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Parametar = "Uv zracenje",
                        Trenutna_vrednost = uv,
                        Granicna_vrednostMin = rezPJ.Granica_svetlost_min,
                        Granicna_vrednostMax = rezPJ.Granica_svetlost_max,
                        Komentar = $"Uv zracenje je van dozvoljenog opsega ({rezPJ.Granica_svetlost_min}-{rezPJ.Granica_svetlost_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));
                }
                await Task.WhenAll(tasks);

            }

        }
    }
}
