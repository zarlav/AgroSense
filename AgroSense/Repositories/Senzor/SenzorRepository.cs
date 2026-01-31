using Cassandra;
using AgroSense.DTOs.Senzor;
namespace AgroSense.Repositories.Senzor
{
    public class SenzorRepository
    {
        private readonly Cassandra.ISession _session;

        public SenzorRepository(Cassandra.ISession session)
        {
            _session = session;
        }

        public async Task DodajSenzor(SenzorCreateDto s)
        {
            var id = Guid.NewGuid();
            var ps = _session.Prepare(@"INSERT INTO senzor 
                  (id_senzora, id_lokacije, naziv, proizvodjac, model, status, vreme_instalacije) 
                  VALUES (?, ?, ?, ?, ?, ?, ?)");
            var rs = await _session.ExecuteAsync(ps.Bind(
                id,
                s.LokacijaId,
                s.Naziv,
                s.Proizvodjac,
                s.Model,
                s.Status,
                s.VremeInstalacije
                ));
        }
        public async Task <SenzorResponseDto?> VratiSenzor(Guid senzorId)
        {
            var ps = _session.Prepare(@"SELECT id_senzora, id_lokacije, naziv, proizvodjac, model, status, vreme_instalacije
                FROM senzor
                WHERE id_senzora = ?");
            var rs = await _session.ExecuteAsync(ps.Bind(senzorId));
            var row = rs.FirstOrDefault();

            if (row == null)
                return null;

            return new SenzorResponseDto
            {
                SenzorId = row.GetValue<Guid>("id_senzora"),
                LokacijaId = row.GetValue<Guid>("id_lokacije"),
                Naziv = row.GetValue<string>("naziv"),
                Proizvodjac = row.GetValue<string>("proizvodjac"),
                Model = row.GetValue<string>("model"),
                Status = row.GetValue<string>("status"),
                VremeInstalacije = row.GetValue<DateTime>("vreme_instalacije")
            };
        }

        public async Task<List<SenzorResponseDto>> VratiSveSenzore()
        {
            var ps = _session.Prepare(@"SELECT id_senzora, id_lokacije, model, naziv, proizvodjac, status, vreme_instalacije FROM senzor");
            var rs = await _session.ExecuteAsync(ps.Bind());

            return rs.Select(row => new SenzorResponseDto
            {
                SenzorId = row.GetValue<Guid>("id_senzora"),
                LokacijaId = row.GetValue<Guid>("id_lokacije"),
                Model = row.GetValue<string>("model"),
                Naziv = row.GetValue<string>("naziv"),
                Proizvodjac = row.GetValue<string>("proizvodjac"),
                Status = row.GetValue<string>("status"),
                VremeInstalacije = row.GetValue<DateTime>("vreme_instalacije")
            }).ToList();
        }

        public async Task<List<SenzorIdDto>> VratiSveIdSenzora()
        {
            var ps = _session.Prepare("SELECT id_senzora, id_lokacije FROM senzor");
            var rs = await _session.ExecuteAsync(ps.Bind());

            return rs.Select(r => new SenzorIdDto
            {
                SenzorId = r.GetValue<Guid>("id_senzora"),
                LokacijaId = r.GetValue<Guid>("id_lokacije")
            }).ToList();
        }
    }
}
