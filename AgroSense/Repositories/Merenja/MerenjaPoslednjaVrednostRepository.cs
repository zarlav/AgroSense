using Cassandra;

namespace AgroSense.Repositories.Merenja
{
    public class MerenjaPoslednjaVrednostRepository
    {
        private readonly Cassandra.ISession _session;


        public MerenjaPoslednjaVrednostRepository(Cassandra.ISession session)
        {
            _session = session;
        }

        public async Task DodajMerenja(DTOs.Merenje.MerenjeCreateDto merenje)
        {
            if (merenje == null)
                return;
            var ps = _session.Prepare(@"INSERT INTO senzor_poslednja_vrednost (id_senzora,
            id_lokacije, dan, ts, temperatura, vlaznost, co2, 
            jacina_vetra, smer_vetra, ph_zemljista, uv_vrednost,vlaznost_lista, pritisak_vazduha, pritisak_u_cevima)
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
        public async Task<DTOs.Merenje.MerenjeResponseDto?> VratiPoslednjeMerenje(Guid senzorID)
        {
            var ps = _session.Prepare(@"SELECT id_senzora, id_lokacije, dan, ts,
                 temperatura, vlaznost, co2, jacina_vetra,
                 smer_vetra, ph_zemljista, uv_vrednost,
                 vlaznost_lista, pritisak_vazduha, pritisak_u_cevima
                 FROM senzor_poslednja_vrednost
                 WHERE id_senzora = ? LIMIT 1");
            var rs = await _session.ExecuteAsync(ps.Bind(senzorID));

            var row = rs.FirstOrDefault();
            if (row == null)
                return null;

            return new DTOs.Merenje.MerenjeResponseDto
            {
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
            };
        }
    }
}
