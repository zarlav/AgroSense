using AgroSense.DTOs.Alarm;
using AgroSense.Services;
using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgroSense.Services
{
    public class AlarmService
    {
        private readonly SenzorService _senzorService;
        private readonly MerenjaService _merenjaService;

        private const float TEMP_MIN = 0;
        private const float TEMP_MAX = 35;

        private const float VLAGA_MIN = 30;
        private const float VLAGA_MAX = 80;

        private const float CO2_MAX = 1000;

        public AlarmService(
            SenzorService senzorService,
            MerenjaService merenjaService)
        {
            _senzorService = senzorService;
            _merenjaService = merenjaService;
        }

        public List<AlarmResponseDTO> GenerisiAlarmeZaDanas(Guid lokacijaId)
        {
            var now = DateTime.UtcNow;
            var danas = new LocalDate(now.Year, now.Month, now.Day);

            var merenja = _merenjaService.VratiMerenjePoLokaciji(
                lokacijaId,
                danas,
                TimeSpan.Zero,
                new TimeSpan(23, 59, 59)
            );

            var alarmi = new List<AlarmResponseDTO>();

            foreach (var m in merenja)
            {
               
                if (m.Temperatura < TEMP_MIN || m.Temperatura > TEMP_MAX)
                {
                    alarmi.Add(new AlarmResponseDTO
                    {
                        LokacijaId = lokacijaId,
                        Parametar = "Temperatura",
                        Vrednost = m.Temperatura,
                        Min = TEMP_MIN,
                        Max = TEMP_MAX,
                        Vreme = m.Ts,
                        Poruka = "Temperatura van dozvoljenog opsega"
                    });
                }

                
                if (m.Vlaznost < VLAGA_MIN || m.Vlaznost > VLAGA_MAX)
                {
                    alarmi.Add(new AlarmResponseDTO
                    {
                        LokacijaId = lokacijaId,
                        Parametar = "Vlažnost",
                        Vrednost = m.Vlaznost,
                        Min = VLAGA_MIN,
                        Max = VLAGA_MAX,
                        Vreme = m.Ts,
                        Poruka = "Vlažnost van dozvoljenog opsega"
                    });
                }

                // CO2
                if (m.Co2 > CO2_MAX)
                {
                    alarmi.Add(new AlarmResponseDTO
                    {
                        LokacijaId = lokacijaId,
                        Parametar = "CO2",
                        Vrednost = m.Co2,
                        Min = 0,
                        Max = CO2_MAX,
                        Vreme = m.Ts,
                        Poruka = "Povećan nivo CO2"
                    });
                }
            }

            return alarmi
                .OrderByDescending(a => a.Vreme)
                .ToList();
        }
    }
}
