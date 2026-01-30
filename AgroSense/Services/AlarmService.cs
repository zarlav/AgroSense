using AgroSense.DTOs.Alarm;
using Cassandra;

namespace AgroSense.Services
{
    public class AlarmService
    {
        private readonly AlarmRepository _repository;

        public AlarmService(AlarmRepository repository)
        {
            _repository = repository;
        }

        public List<AlarmResponseDTO> VratiAlarmePoJedinici(
            Guid idJedinice,
            LocalDate dan,
            DateTime vremeOd,
            DateTime vremeDo)
        {
            var alarmi = _repository.VratiAlarmePoJedinici(
                idJedinice,
                dan,
                vremeOd,
                vremeDo);

            return alarmi.Select(a => new AlarmResponseDTO
            {
                IdAlarma = a.IdAlarma,
                TipSenzora = a.TipSenzora,
                Parametar = a.Parametar,
                TrenutnaVrednost = a.TrenutnaVrednost,
                GranicnaVrednost = a.GranicnaVrednost,
                StanjePre = a.StanjePre,
                StanjePosle = a.StanjePosle,
                Prioritet = a.Prioritet,
                VremeAktivacije = a.VremeDogadjaja,
                Komentar = a.Komentar
            }).ToList();
        }
    }
}
