using AgroSense.DTOs.Alarm;
using AgroSense.DTOs.Merenje;
using Cassandra;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgroSense.Repositories.Alarmi
{
    public class AlarmiPoSenzoruRepository
    {
        private readonly Cassandra.ISession _session;

        public AlarmiPoSenzoruRepository(Cassandra.ISession session)
        {
            _session = session;
        }
        public async Task DodajAlarmPoSenzoru(AlarmCreateDTO adto)
        {
            var ps = _session.Prepare(@"INSERT INTO alarmi_po_senzoru
            (id_senzora, dan, vreme_dogadjaja, id_jedinice, komentar)
            VALUES(?, ?, ?, ?, ?)");
           
            var rs = ps.Bind(
                adto.Id_senzora,
                adto.Dan,
                adto.Vreme_dogadjaja,
                adto.Id_jedinice,
                adto.Komentar
                );

            await _session.ExecuteAsync(rs).ConfigureAwait(false);
        }


        public async Task<List<AlarmResponseDTO>> VratiAlarmePoSenzoru(Guid senzorId, LocalDate dan, TimeSpan _vremeOd, TimeSpan _vremeDo)
        {
            DateTime osnovniDatum = new DateTime(dan.Year, dan.Month, dan.Day);
            DateTime vremeOd = osnovniDatum.Add(_vremeOd);
            DateTime vremeDo = osnovniDatum.Add(_vremeDo);
            var ps = _session.Prepare(@"SELECT dan, vreme_dogadjaja, id_jedinice, komentar FROM alarmi_po_senzoru
            WHERE id_senzora=? AND dan=? AND vreme_dogadjaja >= ? AND vreme_dogadjaja <= ?");

            var rs = await _session.ExecuteAsync(ps.Bind(senzorId, dan, vremeOd, vremeDo));

            return rs.Select(row => new AlarmResponseDTO()
            {
                Dan = row.GetValue<LocalDate>("dan"),
                Vreme_dogadjaja = row.GetValue<DateTime>("vreme_dogadjaja"),
                Id_jedinice = row.GetValue<Guid>("id_jedinice"),
                Komentar = row.GetValue<string>("komentar")
            }).ToList();
        }
    }
}
