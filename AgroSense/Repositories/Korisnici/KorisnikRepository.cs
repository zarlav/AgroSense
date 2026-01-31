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

        public async Task Dodaj(KorisnikCreateDto dto)
        {
            var id = Guid.NewGuid();
            var ps = _session.Prepare(@"INSERT INTO korisnik
                (id_korisnika, ime, kontakt, uloga, aktivan)
                VALUES (?, ?, ?, ?, ?)");
            var rs = await _session.ExecuteAsync(ps.Bind(
                id,
                dto.Ime,
                dto.Kontakt,
                dto.Uloga,
                dto.Aktivan));
        }

        public async Task<List<KorisnikResponseDto>> VratiSve()
        {
            var ps = _session.Prepare(@"SELECT id_korisnika, ime, uloga, aktivan FROM korisnik");
            var rs = await _session.ExecuteAsync(ps.Bind());            

            return rs.Select(r => new KorisnikResponseDto
            {
                Id_korisnika = r.GetValue<Guid>("id_korisnika"),
                Ime = r.GetValue<string>("ime"),
                Uloga = r.GetValue<string>("uloga"),
                Aktivan = r.GetValue<bool>("aktivan")
            }).ToList();
        }

        public async Task<KorisnikViewDto?> VratiPoId(Guid id)
        {
            var ps = _session.Prepare(@"SELECT * FROM korisnik WHERE id_korisnika = ?");
            var rs = await _session.ExecuteAsync(ps.Bind(id));

            var row = rs.FirstOrDefault();
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

        public async Task Update(Guid id, KorisnikCreateDto dto)
        {
            var stmt = new SimpleStatement(@"
                UPDATE korisnik SET
                ime = ?, kontakt = ?, uloga = ?, aktivan = ?
                WHERE id_korisnika = ?",
                dto.Ime, dto.Kontakt, dto.Uloga, dto.Aktivan, id
            );

            await _session.ExecuteAsync(stmt);
        }

        public async Task Delete(Guid id)
        {
            var stmt = new SimpleStatement("DELETE FROM korisnik WHERE id_korisnika = ?", id);
            await _session.ExecuteAsync(stmt);
        }
    }
}
