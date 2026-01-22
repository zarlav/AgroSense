using Cassandra;
using AgroSense.DTOs.ProizvodnaJedinica;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgroSense.Repositories.ProizvodneJedinice
{
    public class ProizvodneJediniceRepository
    {
        private readonly Cassandra.ISession _session;

        public ProizvodneJediniceRepository(Cassandra.ISession session)
        {
            _session = session;
        }

        public void Dodaj(ProizvodnaJedinicaCreateDto dto)
        {
            var id = Guid.NewGuid();
            var ps = _session.Prepare(@"
                INSERT INTO proizvodne_jedinice 
                (tip_jedinice, id_jedinice, naziv, geo_lat, geo_long, povrsina, 
                 granica_temp_min, granica_temp_max, granica_vlaznost_min, granica_vlaznost_max, 
                 granica_vlaznost_lista_min, granica_vlaznost_lista_max, granica_co2_min, granica_co2_max, 
                 granica_jacina_vetra_min, granica_jacina_vetra_max, granica_pritisak_u_cevima_min, 
                 granica_pritisak_u_cevima_max, granica_pritisak_vazduha_min, granica_pritisak_vazduha_max, 
                 granica_svetlost_min, granica_svetlost_max, nadmorska_visina, vrsta_biljaka, 
                 status_mreze, odgovorno_lice, opis, datum_postavljanja, aktivno) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, toTimestamp(now()), true)");

            _session.Execute(ps.Bind(
                dto.Tip_jedinice, id, dto.Naziv, dto.Geo_lat, dto.Geo_long, dto.Povrsina,
                dto.Granica_temp_min, dto.Granica_temp_max, dto.Granica_vlaznost_min, dto.Granica_vlaznost_max,
                dto.Granica_vlaznost_lista_min, dto.Granica_vlaznost_lista_max, dto.Granica_co2_min, dto.Granica_co2_max,
                dto.Granica_jacina_vetra_min, dto.Granica_jacina_vetra_max, dto.Granica_pritisak_u_cevima_min,
                dto.Granica_pritisak_u_cevima_max, dto.Granica_pritisak_vazduha_min, dto.Granica_pritisak_vazduha_max,
                dto.Granica_svetlost_min, dto.Granica_svetlost_max, dto.Nadmorska_visina, dto.Vrsta_biljaka,
                dto.Status_mreze, dto.Odgovorno_lice, dto.Opis
            ));
        }

        public List<ProizvodnaJedinicaResponseDto> VratiSve()
        {
            // ISPRAVLJENO: Dodate su kolone vrsta_biljaka i odgovorno_lice u SELECT
            var rs = _session.Execute("SELECT tip_jedinice, id_jedinice, naziv, povrsina, aktivno, vrsta_biljaka, odgovorno_lice FROM proizvodne_jedinice");

            return rs.Select(r => new ProizvodnaJedinicaResponseDto
            {
                Tip_jedinice = r.GetValue<string>("tip_jedinice"),
                Id_jedinice = r.GetValue<Guid>("id_jedinice"),
                Naziv = r.GetValue<string>("naziv"),
                Povrsina = r.GetValue<float>("povrsina"),
                Aktivno = r.GetValue<bool>("aktivno"),
                // ISPRAVLJENO: Dodato mapiranje vrednosti iz baze u DTO
                Vrsta_biljaka = r.GetValue<string>("vrsta_biljaka"),
                Odgovorno_lice = r.GetValue<string>("odgovorno_lice")
            }).ToList();
        }

        public ProizvodnaJedinicaViewDto? VratiPoId(string tip, Guid id)
        {
            var ps = _session.Prepare("SELECT * FROM proizvodne_jedinice WHERE tip_jedinice=? AND id_jedinice=?");
            var row = _session.Execute(ps.Bind(tip, id)).FirstOrDefault();

            if (row == null) return null;

            return new ProizvodnaJedinicaViewDto
            {
                Tip_jedinice = row.GetValue<string>("tip_jedinice"),
                Id_jedinice = row.GetValue<Guid>("id_jedinice"),
                Naziv = row.GetValue<string>("naziv"),
                Geo_lat = row.GetValue<double>("geo_lat"),
                Geo_long = row.GetValue<double>("geo_long"),
                Povrsina = row.GetValue<float>("povrsina"),
                Granica_temp_min = row.GetValue<float>("granica_temp_min"),
                Granica_temp_max = row.GetValue<float>("granica_temp_max"),
                Granica_vlaznost_min = row.GetValue<float>("granica_vlaznost_min"),
                Granica_vlaznost_max = row.GetValue<float>("granica_vlaznost_max"),
                Granica_vlaznost_lista_min = row.GetValue<float>("granica_vlaznost_lista_min"),
                Granica_vlaznost_lista_max = row.GetValue<float>("granica_vlaznost_lista_max"),
                Granica_co2_min = row.GetValue<float>("granica_co2_min"),
                Granica_co2_max = row.GetValue<float>("granica_co2_max"),
                Granica_jacina_vetra_min = row.GetValue<float>("granica_jacina_vetra_min"),
                Granica_jacina_vetra_max = row.GetValue<float>("granica_jacina_vetra_max"),
                Granica_pritisak_u_cevima_min = row.GetValue<float>("granica_pritisak_u_cevima_min"),
                Granica_pritisak_u_cevima_max = row.GetValue<float>("granica_pritisak_u_cevima_max"),
                Granica_pritisak_vazduha_min = row.GetValue<float>("granica_pritisak_vazduha_min"),
                Granica_pritisak_vazduha_max = row.GetValue<float>("granica_pritisak_vazduha_max"),
                Granica_svetlost_min = row.GetValue<float>("granica_svetlost_min"),
                Granica_svetlost_max = row.GetValue<float>("granica_svetlost_max"),
                Nadmorska_visina = row.GetValue<double>("nadmorska_visina"),
                Vrsta_biljaka = row.GetValue<string>("vrsta_biljaka"),
                Status_mreze = row.GetValue<string>("status_mreze"),
                Odgovorno_lice = row.GetValue<string>("odgovorno_lice"),
                Opis = row.GetValue<string>("opis"),
                Datum_postavljanja = row.GetValue<DateTime>("datum_postavljanja"),
                Aktivno = row.GetValue<bool>("aktivno")
            };
        }

        public void Update(string tip, Guid id, ProizvodnaJedinicaUpdateDto dto)
        {
            var ps = _session.Prepare(@"
                UPDATE proizvodne_jedinice 
                SET naziv=?, geo_lat=?, geo_long=?, povrsina=?, granica_temp_min=?, granica_temp_max=?, 
                    granica_vlaznost_min=?, granica_vlaznost_max=?, granica_vlaznost_lista_min=?, 
                    granica_vlaznost_lista_max=?, granica_co2_min=?, granica_co2_max=?, 
                    granica_jacina_vetra_min=?, granica_jacina_vetra_max=?, granica_pritisak_u_cevima_min=?, 
                    granica_pritisak_u_cevima_max=?, granica_pritisak_vazduha_min=?, granica_pritisak_vazduha_max=?, 
                    granica_svetlost_min=?, granica_svetlost_max=?, vrsta_biljaka=?, status_mreze=?, 
                    nadmorska_visina=?, odgovorno_lice=?, opis=?, aktivno=?
                WHERE tip_jedinice=? AND id_jedinice=?");

            _session.Execute(ps.Bind(
                dto.Naziv, dto.Geo_lat, dto.Geo_long, dto.Povrsina, dto.Granica_temp_min, dto.Granica_temp_max,
                dto.Granica_vlaznost_min, dto.Granica_vlaznost_max, dto.Granica_vlaznost_lista_min,
                dto.Granica_vlaznost_lista_max, dto.Granica_co2_min, dto.Granica_co2_max,
                dto.Granica_jacina_vetra_min, dto.Granica_jacina_vetra_max, dto.Granica_pritisak_u_cevima_min,
                dto.Granica_pritisak_u_cevima_max, dto.Granica_pritisak_vazduha_min, dto.Granica_pritisak_vazduha_max,
                dto.Granica_svetlost_min, dto.Granica_svetlost_max, dto.Vrsta_biljaka, dto.Status_mreze,
                dto.Nadmorska_visina, dto.Odgovorno_lice, dto.Opis, dto.Aktivno,
                tip, id
            ));
        }

        public void Obrisi(string tip, Guid id)
        {
            var ps = _session.Prepare("UPDATE proizvodne_jedinice SET aktivno=false WHERE tip_jedinice=? AND id_jedinice=?");
            _session.Execute(ps.Bind(tip, id));
        }

        public List<Guid> VratiSveIdjeve()
        {
            var rs = _session.Execute("SELECT id_jedinice FROM proizvodne_jedinice WHERE aktivno=true ALLOW FILTERING");
            return rs.Select(r => r.GetValue<Guid>("id_jedinice")).ToList();
        }
    }
}