using Cassandra;
using AgroSense.DTOs.ProizvodnaJedinica;

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

            var stmt = new SimpleStatement(@"
                INSERT INTO proizvodne_jedinice
                (id_jedinice, naziv, geo_lat, geo_long, tip_jedinice, povrsina,
                 granica_temp_min, granica_temp_max,
                 granica_vlaznost_min, granica_vlaznost_max,
                 vrsta_biljaka, status_mreze, nadmorska_visina,
                 odgovorno_lice, opis, datum_postavljanja, aktivno)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, toTimestamp(now()), true)",
                id,
                dto.Naziv,
                dto.Geo_lat,
                dto.Geo_long,
                dto.Tip_jedinice,
                dto.Povrsina,
                dto.Granica_temp_min,
                dto.Granica_temp_max,
                dto.Granica_vlaznost_min,
                dto.Granica_vlaznost_max,
                dto.Vrsta_biljaka,
                dto.Status_mreze,
                dto.Nadmorska_visina,
                dto.Odgovorno_lice,
                dto.Opis
            );

            _session.Execute(stmt);
        }

        public List<ProizvodnaJedinicaResponseDto> VratiSve()
        {
            var stmt = new SimpleStatement(@"
                SELECT id_jedinice, naziv, tip_jedinice, povrsina, aktivno
                FROM proizvodne_jedinice");

            var rows = _session.Execute(stmt);

            return rows.Select(r => new ProizvodnaJedinicaResponseDto
            {
                Id_jedinice = r.GetValue<Guid>("id_jedinice"),
                Naziv = r.GetValue<string>("naziv"),
                Tip_jedinice = r.GetValue<string>("tip_jedinice"),
                Povrsina = r.GetValue<float>("povrsina"),
                Aktivno = r.GetValue<bool>("aktivno")
            }).ToList();
        }

        public ProizvodnaJedinicaViewDto? VratiPoId(Guid id)
        {
            var stmt = new SimpleStatement(@"
               SELECT id_jedinice, naziv, geo_lat, geo_long, tip_jedinice, povrsina,
               granica_temp_min, granica_temp_max,
               granica_vlaznost_min, granica_vlaznost_max,
               vrsta_biljaka, status_mreze, nadmorska_visina,
               odgovorno_lice, opis, datum_postavljanja, aktivno
               FROM proizvodne_jedinice
               WHERE id_jedinice = ?",
                id
            );

            var row = _session.Execute(stmt).FirstOrDefault();
            if (row == null) return null;

            return new ProizvodnaJedinicaViewDto
            {
                Id_jedinice = row.GetValue<Guid>("id_jedinice"),
                Naziv = row.GetValue<string>("naziv"),
                Geo_lat = row.GetValue<double>("geo_lat"),
                Geo_long = row.GetValue<double>("geo_long"),
                Tip_jedinice = row.GetValue<string>("tip_jedinice"),
                Povrsina = row.GetValue<float>("povrsina"),

                Granica_temp_min = row.GetValue<float>("granica_temp_min"),
                Granica_temp_max = row.GetValue<float>("granica_temp_max"),
                Granica_vlaznost_min = row.GetValue<float>("granica_vlaznost_min"),
                Granica_vlaznost_max = row.GetValue<float>("granica_vlaznost_max"),

                Vrsta_biljaka = row.GetValue<string>("vrsta_biljaka"),
                Status_mreze = row.GetValue<string>("status_mreze"),
                Nadmorska_visina = row.GetValue<double>("nadmorska_visina"),
                Odgovorno_lice = row.GetValue<string>("odgovorno_lice"),
                Opis = row.GetValue<string>("opis"),
                Datum_postavljanja = row.GetValue<DateTime>("datum_postavljanja"),
                Aktivno = row.GetValue<bool>("aktivno")
            };
        }
        public void Update(Guid id, ProizvodnaJedinicaUpdateDto dto)
        {
            var stmt = new SimpleStatement(@"
        UPDATE proizvodne_jedinice
        SET naziv = ?,
            geo_lat = ?,
            geo_long = ?,
            tip_jedinice = ?,
            povrsina = ?,
            granica_temp_min = ?,
            granica_temp_max = ?,
            granica_vlaznost_min = ?,
            granica_vlaznost_max = ?,
            vrsta_biljaka = ?,
            status_mreze = ?,
            nadmorska_visina = ?,
            odgovorno_lice = ?,
            opis = ?,
            aktivno = ?
        WHERE id_jedinice = ?",
                dto.Naziv,
                dto.Geo_lat,
                dto.Geo_long,
                dto.Tip_jedinice,
                dto.Povrsina,
                dto.Granica_temp_min,
                dto.Granica_temp_max,
                dto.Granica_vlaznost_min,
                dto.Granica_vlaznost_max,
                dto.Vrsta_biljaka,
                dto.Status_mreze,
                dto.Nadmorska_visina,
                dto.Odgovorno_lice,
                dto.Opis,
                dto.Aktivno,
                id
            );

            _session.Execute(stmt);
        }
        public void Obrisi(Guid id)
        {
            var stmt = new SimpleStatement(@"
        UPDATE proizvodne_jedinice
        SET aktivno = false
        WHERE id_jedinice = ?",
                id
            );

            _session.Execute(stmt);
        }
        public List<Guid> VratiSveIdjeve()
        {
            var stmt = new SimpleStatement(@"
        SELECT id_jedinice
        FROM proizvodne_jedinice
        WHERE aktivno = true ALLOW FILTERING");

            var rows = _session.Execute(stmt);

            return rows
                .Select(r => r.GetValue<Guid>("id_jedinice"))
                .ToList();
        }

    }
}
