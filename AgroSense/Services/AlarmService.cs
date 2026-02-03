using AgroSense.DTOs.Alarm;
using AgroSense.Repositories.Alarmi;
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
        private readonly AlarmiPoSenzoruRepository _repositoryAlarmiPoSenzoru;

        public AlarmService(AlarmRepository repository, ProizvodneJediniceRepository repositoryProizvodnaJedinica, MerenjaPoLokacijiRepository repositoryPoLokaciji, AlarmiPoSenzoruRepository repositoryAlarmiPoSenzoru)
        {
            _repository = repository;
            _repositoryProizvodnaJedinica = repositoryProizvodnaJedinica;
            _repositoryPoLokaciji = repositoryPoLokaciji;
            _repositoryAlarmiPoSenzoru = repositoryAlarmiPoSenzoru;
        }

        public async Task<List<AlarmResponseDTO>> VratiAlarmePoJedinici(
            Guid idJedinice,
            LocalDate dan,
            TimeSpan vremeOd,
            TimeSpan vremeDo)
        {
            return await _repository.VratiAlarmePoJedinici(idJedinice, dan, vremeOd, vremeDo);
        }

        public async Task<List<AlarmPoSenzoruResponseDTO>> VratiAlarmePoSenzoru(Guid senzorId, LocalDate dan, TimeSpan _vremeOd, TimeSpan _vremeDo)
        {
            return await _repositoryAlarmiPoSenzoru.VratiAlarmePoSenzoru(senzorId, dan, _vremeOd, _vremeDo);
        }

        public async Task ProveriAlarm(Guid idJedinice, bool stanje, DateTime _vremeSlanja)
        {
            var localDate = new Cassandra.LocalDate(_vremeSlanja.Year, _vremeSlanja.Month, _vremeSlanja.Day);
            var vremeSlanja = _vremeSlanja.TimeOfDay;
            var vremePreMinuta = vremeSlanja - TimeSpan.FromMinutes(1);
            var vremeNakonMinuta = vremeSlanja + TimeSpan.FromMinutes(1);

            Console.WriteLine($"Vreme kada je poslato {vremeSlanja} /n vreme pre minuta: {vremePreMinuta} / vreme nakon minuta {vremeNakonMinuta}");

            var tip = await _repositoryProizvodnaJedinica.VratiTipAktivneJedinice(idJedinice);
            if (tip == null)
                throw new InvalidOperationException($"Ne postoji aktivna proizvodna jedinica sa ID = {idJedinice}");
            var rezPJ = await _repositoryProizvodnaJedinica.VratiPoId(tip.ToString(), stanje, idJedinice);
            if (rezPJ == null)
                throw new InvalidOperationException($"Ne postoji aktivna proizvodna jedinica sa ID = {idJedinice}");
            var rezMPL = await _repositoryPoLokaciji.VratiMerenjaPoLokaciji(
                idJedinice,
                localDate,
                vremePreMinuta,
                vremeNakonMinuta
            );
            if (rezMPL == null)
                throw new InvalidOperationException($"Nije pronadjeno merenje po lokaciji!!!");
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
                var tasks = new List<Task>();

                if (temp < rezPJ.Granica_temp_min || temp > rezPJ.Granica_temp_max)
                {
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
                    var acdto1 = new AlarmCreateDTO()
                    {
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Dan = poslednjeMerenje.Datum,
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_jedinice = idJedinice,
                        Komentar = $"Temperatura je van dozvoljenog opsega ({rezPJ.Granica_temp_min}-{rezPJ.Granica_temp_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));
                    tasks.Add(_repositoryAlarmiPoSenzoru.DodajAlarmPoSenzoru(acdto1));

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
                    var acdto1 = new AlarmCreateDTO()
                    {
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Dan = poslednjeMerenje.Datum,
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_jedinice = idJedinice,
                        Komentar = $"CO2 je van dozvoljenog opsega ({rezPJ.Granica_co2_min}-{rezPJ.Granica_co2_max})"
                    };
                    tasks.Add(_repository.KreirajAlarm(acdto));
                    tasks.Add(_repositoryAlarmiPoSenzoru.DodajAlarmPoSenzoru(acdto1));
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
                    var acdto1 = new AlarmCreateDTO()
                    {
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Dan = poslednjeMerenje.Datum,
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_jedinice = idJedinice,
                        Komentar = $"Vlaznost lista je van dozvoljenog opsega ({rezPJ.Granica_vlaznost_lista_min}-{rezPJ.Granica_vlaznost_lista_max})"
                    };
                     tasks.Add(_repository.KreirajAlarm(acdto));
                     tasks.Add(_repositoryAlarmiPoSenzoru.DodajAlarmPoSenzoru(acdto1));
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
                    var acdto1 = new AlarmCreateDTO()
                    {
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Dan = poslednjeMerenje.Datum,
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_jedinice = idJedinice,
                        Komentar = $"Vlaznost vazduha je van dozvoljenog opsega ({rezPJ.Granica_vlaznost_min}-{rezPJ.Granica_vlaznost_max})"
                    };
                        tasks.Add(_repository.KreirajAlarm(acdto));
                        tasks.Add(_repositoryAlarmiPoSenzoru.DodajAlarmPoSenzoru(acdto1));
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
                    var acdto1 = new AlarmCreateDTO()
                    {
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Dan = poslednjeMerenje.Datum,
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_jedinice = idJedinice,
                        Komentar = $"Pritisak vazduha je van dozvoljenog opsega ({rezPJ.Granica_pritisak_vazduha_min}-{rezPJ.Granica_pritisak_vazduha_max})"
                    };
                        tasks.Add(_repository.KreirajAlarm(acdto));
                        tasks.Add(_repositoryAlarmiPoSenzoru.DodajAlarmPoSenzoru(acdto1));
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
                    var acdto1 = new AlarmCreateDTO()
                    {
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Dan = poslednjeMerenje.Datum,
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_jedinice = idJedinice,
                        Komentar = $"Pritisak u cevima je van dozvoljenog opsega ({rezPJ.Granica_pritisak_u_cevima_min}-{rezPJ.Granica_pritisak_u_cevima_max})"
                    };
                        tasks.Add(_repository.KreirajAlarm(acdto));
                        tasks.Add(_repositoryAlarmiPoSenzoru.DodajAlarmPoSenzoru(acdto1));
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
                    var acdto1 = new AlarmCreateDTO()
                    {
                        Id_senzora = poslednjeMerenje.Id_senzora,
                        Dan = poslednjeMerenje.Datum,
                        Vreme_dogadjaja = poslednjeMerenje.Ts,
                        Id_jedinice = idJedinice,
                        Komentar = $"Uv zracenje je van dozvoljenog opsega ({rezPJ.Granica_svetlost_min}-{rezPJ.Granica_svetlost_max})"
                    };
                        tasks.Add(_repository.KreirajAlarm(acdto));
                        tasks.Add(_repositoryAlarmiPoSenzoru.DodajAlarmPoSenzoru(acdto1));

                }
                await Task.WhenAll(tasks);

            }

        }
    }
}
