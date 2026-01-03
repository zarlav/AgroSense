using AgroSense.Domain;
using Cassandra;

namespace AgroSense.Repositories.Merenja
{
    public class MerenjaPoLokacijiRepository
    {
        private readonly Cassandra.ISession _session;
        public MerenjaPoLokacijiRepository(Cassandra.ISession session)
        {
            _session = session;
        }
        public void DodajMerenja(DTOs.Merenje.MerenjeCreateDto merenje)
        {
            if (merenje == null)
                return;

            var stmt = new SimpleStatement(
                @"INSERT INTO senzor_podatak_po_lokaciji
          (id_senzora, id_lokacije, dan, ts, temperatura, vlaznost, co2,
           jacina_vetra, smer_vetra, ph_zemljista, uv_vrednost,
           vlaznost_lista, pritisak_vazduha, pritisak_u_cevima)
          VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                merenje.Id_senzora,
                merenje.Id_loakcije,
                merenje.Datum,
                merenje.Ts,
                merenje.Temperatura,
                merenje.Vlaznost,
                merenje.Co2,
                merenje.Jacina_vetra,
                merenje.smer_vetra,
                merenje.Ph_zemljista,
                merenje.Uv_vrednost,
                merenje.Vlaznost_lista,
                merenje.Pritisak_vazduha,
                merenje.Pritisak_u_cevima
            );

            _session.Execute(stmt);
        }
        public List<DTOs.Merenje.MerenjeResponseDto> VratiMerenjaPoLokaciji(Guid lokacijaId, LocalDate dan)
        {
            var stmt = new SimpleStatement(
                @"SELECT id_senzora, id_lokacije, dan, ts,
                 temperatura, vlaznost, co2, jacina_vetra,
                 smer_vetra, ph_zemljista, uv_vrednost,
                 vlaznost_lista, pritisak_vazduha, pritisak_u_cevima
          FROM senzor_podatak_po_lokaciji
          WHERE id_lokacije = ? AND dan = ?",
                lokacijaId, dan
            );

            var rows = _session.Execute(stmt);

            var rezultat = new List<DTOs.Merenje.MerenjeResponseDto>();

            foreach (var row in rows)
            {
                rezultat.Add(new DTOs.Merenje.MerenjeResponseDto
                {
                   // Id_senzora = row.GetValue<Guid>("id_senzora"),
                   // Id_loakcije = row.GetValue<Guid>("id_lokacije"),
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
                });
            }

            return rezultat;
        }

       
    }
}
