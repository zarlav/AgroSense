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
                INSERT INTO korisnik
                (id_korisnika, ime, kontakt, uloga, aktivan)
                VALUES (?, ?, ?, ?, ?)",
                id, dto.Ime, dto.Kontakt, dto.Uloga, dto.Aktivan
            );

            _session.Execute(stmt);
        }

        public List<KorisnikResponseDto> VratiSve()
        {
            var stmt = new SimpleStatement("SELECT id_korisnika, ime, uloga, aktivan FROM korisnik");
            var rows = _session.Execute(stmt);

            return rows.Select(r => new KorisnikResponseDto
            {
                Id_korisnika = r.GetValue<Guid>("id_korisnika"),
                Ime = r.GetValue<string>("ime"),
                Uloga = r.GetValue<string>("uloga"),
                Aktivan = r.GetValue<bool>("aktivan")
            }).ToList();
        }

        public KorisnikViewDto? VratiPoId(Guid id)
        {
            var stmt = new SimpleStatement("SELECT * FROM korisnik WHERE id_korisnika = ?", id);
            var row = _session.Execute(stmt).FirstOrDefault();
            if (row == null) return null;

            return new KorisnikViewDto
            {
                Id_korisnika = row.GetValue<Guid>("id_korisnika"),
                Ime = row.GetValue<string>("ime"),
                Kontakt = row.GetValue<string>("kontakt"),
                Uloga = row.GetValue<string>("uloga"),
                Aktivan = row.GetValue<bool>("aktivan")
            };
        }

        public void Update(Guid id, KorisnikCreateDto dto)
        {
            var stmt = new SimpleStatement(@"
                UPDATE korisnik SET
                ime = ?, kontakt = ?, uloga = ?, aktivan = ?
                WHERE id_korisnika = ?",
                dto.Ime, dto.Kontakt, dto.Uloga, dto.Aktivan, id
            );

            _session.Execute(stmt);
        }

        public void Delete(Guid id)
        {
            var stmt = new SimpleStatement("DELETE FROM korisnik WHERE id_korisnika = ?", id);
            _session.Execute(stmt);
        }
    }
}
