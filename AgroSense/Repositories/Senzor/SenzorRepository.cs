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

        public void DodajSenzor(SenzorCreateDto s)
        {
            var id = Guid.NewGuid();
            var stmt = new SimpleStatement(
                @"INSERT INTO senzor 
                  (id_senzora, id_lokacije, naziv, proizvodjac, model, status, vreme_instalacije) 
                  VALUES (?, ?, ?, ?, ?, ?, ?)",
                id,
                s.LokacijaId,
                s.Naziv,
                s.Proizvodjac,
                s.Model,
                s.Status,
                s.VremeInstalacije
            );
            _session.Execute(stmt);
        }
        public SenzorResponseDto VratiSenzor(Guid senzorId)
        {
            var stmt = new SimpleStatement(
            @"SELECT id_senzora, id_lokacije, naziv, proizvodjac, model, status, vreme_instalacije
                FROM senzor
                WHERE id_senzora = ?", senzorId);

            var row = _session.Execute(stmt).FirstOrDefault();

            if (row == null)
                return null;

            return new SenzorResponseDto
            {
                LokacijaId = row.GetValue<Guid>("id_lokacije"),
                Naziv = row.GetValue<string>("naziv"),
                Proizvodjac = row.GetValue<string>("proizvodjac"),
                Model = row.GetValue<string>("model"),
                Status = row.GetValue<string>("status"),
                VremeInstalacije = row.GetValue<DateTime>("vreme_instalacije")
            };
        }

        public List<SenzorResponseDto> VratiSveSenzore()
        {
            var stmt = new SimpleStatement(@"SELECT id_senzora,id_lokacije, model, naziv,proizvodjac, status, vreme_instalacije FROM senzor");
            var rows = _session.Execute(stmt);
            var result = new List<SenzorResponseDto>();
            foreach (var row in rows)
            { 
                result.Add(new SenzorResponseDto 
                {
                    SenzorId = row.GetValue<Guid>("id_senzora"),
                    LokacijaId = row.GetValue<Guid>("id_lokacije"),
                    Model = row.GetValue<string>("model"), 
                    Naziv = row.GetValue<string>("naziv"), 
                    Proizvodjac = row.GetValue<string>("proizvodjac"), 
                    Status = row.GetValue<string>("status"), 
                    VremeInstalacije = row.GetValue<DateTime>("vreme_instalacije") 
                });
            }
            return result;
        }

        public List<Guid> VratiSveIdSenzora()
        {
            var stmt = new SimpleStatement("SELECT id_senzora FROM senzor");

            var rows = _session.Execute(stmt);

            return rows
                .Select(r => r.GetValue<Guid>("id_senzora"))
                .ToList();
        }
    }
}
