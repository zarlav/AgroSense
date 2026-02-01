using AgroSense.Domain;
using Cassandra;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgroSense.Repositories.Merenja
{
    public class MerenjaPoLokacijiRepository
    {
        private readonly Cassandra.ISession _session;
        public MerenjaPoLokacijiRepository(Cassandra.ISession session)
        {
            _session = session;
        }
        public async Task DodajMerenja(DTOs.Merenje.MerenjeCreateDto merenje)
        {
            if (merenje == null)
                return;
            var ps = _session.Prepare(@"INSERT INTO senzor_podatak_po_lokaciji
            (id_senzora, id_lokacije, dan, ts, temperatura, vlaznost, co2,
            jacina_vetra, smer_vetra, ph_zemljista, uv_vrednost,
            vlaznost_lista, pritisak_vazduha, pritisak_u_cevima)
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

            var rs = await _session.ExecuteAsync(ps.Bind(
                merenje.Id_senzora,
                merenje.Id_lokacije,
                merenje.Datum,
                merenje.Ts,
                merenje.Temperatura,
                merenje.Vlaznost,
                merenje.Co2,
                merenje.Jacina_vetra,
                merenje.Smer_vetra,
                merenje.Ph_zemljista,
                merenje.Uv_vrednost,
                merenje.Vlaznost_lista,
                merenje.Pritisak_vazduha,
                merenje.Pritisak_u_cevima));
        }
        public async Task<List<DTOs.Merenje.MerenjeResponseDto>> VratiMerenjaPoLokaciji(Guid lokacijaId, LocalDate dan, TimeSpan vremeOd, TimeSpan vremeDo)
        {
            DateTime osnovniDatum = new DateTime(dan.Year, dan.Month, dan.Day);
            DateTime pocetak = osnovniDatum.Add(vremeOd);
            DateTime kraj = osnovniDatum.Add(vremeDo);

            var ps = _session.Prepare(@"
                SELECT id_lokacije,id_senzora, dan, ts,
                temperatura, vlaznost, co2, jacina_vetra,
                smer_vetra, ph_zemljista, uv_vrednost,
                vlaznost_lista, pritisak_vazduha, pritisak_u_cevima
                FROM senzor_podatak_po_lokaciji
                WHERE id_lokacije = ? AND dan = ? AND ts >= ? AND ts <= ? LIMIT 100");

            var rs = await _session.ExecuteAsync(ps.Bind(lokacijaId, dan, pocetak, kraj));

            return rs.Select(row => new DTOs.Merenje.MerenjeResponseDto
            {
                Id_senzora = row.GetValue<Guid>("id_senzora"),
                Datum = row.GetValue<LocalDate>("dan"),
                Ts = row.GetValue<DateTime>("ts"),
                Temperatura = row.GetValue<float>("temperatura"),
                Vlaznost = row.GetValue<float>("vlaznost"),
                Co2 = row.GetValue<float>("co2"),
                Jacina_vetra = row.GetValue<float>("jacina_vetra"),
                smer_vetra = row.GetValue<string>("smer_vetra"),
                Ph_zemljista = row.GetValue<float>("ph_zemljista"),
                Uv_vrednost = row.GetValue<float>("uv_vrednost"),
                Vlaznost_lista = row.GetValue<float>("vlaznost_lista"),
                Pritisak_vazduha = row.GetValue<float>("pritisak_vazduha"),
                Pritisak_u_cevima = row.GetValue<float>("pritisak_u_cevima")
            }).ToList();
        }

    }
}
