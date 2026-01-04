using AgroSense.Domain;
using AgroSense.Services;
using Cassandra;
using AgroSense.DTOs.Merenje;

namespace AgroSense.Repositories.Merenja
{
    public class MerenjaPoDanuRepository
    {
        private readonly Cassandra.ISession _session;

        public MerenjaPoDanuRepository(Cassandra.ISession session)
        {
            _session = session;
        }

        public void DodajMerenja(MerenjeCreateDto merenje)
        {
            if (merenje == null) return;

            var query = new SimpleStatement(@"
                INSERT INTO senzor_podatak_po_danu 
                (id_senzora, id_lokacije, dan, ts, temperatura, vlaznost, co2, jacina_vetra,
                 smer_vetra, ph_zemljista, uv_vrednost, vlaznost_lista, pritisak_vazduha, pritisak_u_cevima)
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

            _session.Execute(query);
        }

        public List<MerenjeResponseDto> VratiMerenjaPoDanu(Guid senzorId, LocalDate dan)
        {
            var query = new SimpleStatement(@"
                SELECT id_senzora, id_lokacije, dan, ts, temperatura, vlaznost, co2, jacina_vetra,
                       smer_vetra, ph_zemljista, uv_vrednost, vlaznost_lista, pritisak_vazduha, pritisak_u_cevima
                FROM senzor_podatak_po_danu
                WHERE dan = ? AND id_senzora = ?
                ORDER BY ts DESC",
                dan, senzorId
            );

            var rows = _session.Execute(query);
            return rows.Select(row => new MerenjeResponseDto
            {
             //   Id_senzora = row.GetValue<Guid>("id_senzora"),
             //   Id_loakcije = row.GetValue<Guid>("id_lokacije"),
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
        public List<MerenjeResponseDto> VratiMerenjaPoVremeneskomOpsegu(Guid senzorId, LocalDate datum, TimeSpan vremeOd, TimeSpan vremeDo)
        {
            DateTime osnovniDatum = new DateTime(datum.Year, datum.Month, datum.Day);
            DateTime pocetak = osnovniDatum.Add(vremeOd);
            DateTime kraj = osnovniDatum.Add(vremeDo);
            var stmt = new SimpleStatement(
            @"SELECT id_senzora, id_lokacije, dan, ts,
                 temperatura, vlaznost, co2, jacina_vetra,
                 smer_vetra, ph_zemljista, uv_vrednost,
                 vlaznost_lista, pritisak_vazduha, pritisak_u_cevima
            FROM senzor_podatak_po_danu
            WHERE id_senzora = ? AND dan = ? AND ts >= ? AND ts <= ?", senzorId, datum, pocetak, kraj);

            var rows = _session.Execute(stmt);

            var rezultat = new List<MerenjeResponseDto>();

            foreach (var row in rows)
            {
                rezultat.Add(new MerenjeResponseDto
                {
                //    Id_senzora = row.GetValue<Guid>("id_senzora"),
                //    Id_loakcije = row.GetValue<Guid>("id_lokacije"),
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
   