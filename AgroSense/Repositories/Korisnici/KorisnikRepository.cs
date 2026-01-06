using Cassandra;
using AgroSense.DTOs.Korisnik;

namespace AgroSense.Repositories.Korisnici
{
    public class KorisnikRepository
    {
        private readonly Cassandra.ISession _session;

        public KorisnikRepository(Cassandra.ISession session)
        {
            _session = session;
        }

        public void Dodaj(KorisnikCreateDto dto)
        {
            var id = Guid.NewGuid();

            var stmt = new SimpleStatement(@"
                INSERT INTO korisnici
                (id_korisnika, ime, prezime, email, lozinka, uloga, aktivan, datum_registracije)
                VALUES (?, ?, ?, ?, ?, 'USER', true, toTimestamp(now()))",
                id,
                dto.Ime,
                dto.Prezime,
                dto.Email,
                dto.Lozinka
            );

            _session.Execute(stmt);
        }

        public List<KorisnikResponseDto> VratiSve()
        {
            var stmt = new SimpleStatement(@"
                SELECT id_korisnika, ime, prezime, email, aktivan
                FROM korisnici");

            var rows = _session.Execute(stmt);

            return rows.Select(r => new KorisnikResponseDto
            {
                Id_korisnika = r.GetValue<Guid>("id_korisnika"),
                Ime = r.GetValue<string>("ime"),
                Prezime = r.GetValue<string>("prezime"),
                Email = r.GetValue<string>("email"),
                Aktivan = r.GetValue<bool>("aktivan")
            }).ToList();
        }

        public KorisnikViewDto? VratiPoId(Guid id)
        {
            var stmt = new SimpleStatement(@"
                SELECT *
                FROM korisnici
                WHERE id_korisnika = ?",
                id
            );

            var row = _session.Execute(stmt).FirstOrDefault();
            if (row == null) return null;

            return new KorisnikViewDto
            {
                Id_korisnika = row.GetValue<Guid>("id_korisnika"),
                Ime = row.GetValue<string>("ime"),
                Prezime = row.GetValue<string>("prezime"),
                Email = row.GetValue<string>("email"),
                Uloga = row.GetValue<string>("uloga"),
                Aktivan = row.GetValue<bool>("aktivan"),
                Datum_registracije = row.GetValue<DateTime>("datum_registracije")
            };
        }
    }
}
