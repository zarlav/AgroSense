using AgroSense.QueryEntities;
using AgroSense.Services;
using Cassandra;

namespace AgroSense.Repositories
{
    public class ProizvodneJediniceRepository
    {
        public static ProizvodnaJedinica? GetProizvodnaJedinica(Guid idProizvodneJedinice)
        {
            Cassandra.ISession session = SessionManager.GetSession();

            var statement = new SimpleStatement(
                "SELECT * FROM proizvodne_jedinice WHERE id_jedinice = ?",
                idProizvodneJedinice
            );

            Row? row = session.Execute(statement).FirstOrDefault();
            if (row == null)
                return null;

            return new ProizvodnaJedinica
            {
                IdJedinice = row.GetValue<Guid>("id_jedinice"),
                Naziv = row.GetValue<string>("naziv"),
                GeoLat = row.GetValue<double>("geo_lat"),
                GeoLong = row.GetValue<double>("geo_long"),
                TipJedinice = row.GetValue<string>("tip_jedinice"),
                Povrsina = row.GetValue<double>("povrsina"),
                Status = row.GetValue<string>("status"),
                Aktivno = row.GetValue<bool>("aktivno"),
                VrstaBiljaka = row.GetValue<string>("vrsta_biljaka"),
                BrojSenzora = row.GetValue<int>("broj_senzora"),
                StatusMreze = row.GetValue<string>("status_mreze"),
                PoslednjiUpdate = row.GetValue<DateTime>("poslednji_update"),
                DatumPostavljanja = row.GetValue<DateTime>("datum_postavljanja"),
                Opis = row.GetValue<string>("opis"),
                OdgovornoLice = row.GetValue<string>("odgovorno_lice"),
                NadmorskaVisina = row.GetValue<double>("nadmorska_visina"),
                GranicaTempMax = row.GetValue<double>("granica_temp_max"),
                GranicaTempMin = row.GetValue<double>("granica_temp_min"),
                GranicaVlagaMax = row.GetValue<double>("granica_vlaga_max"),
                GranicaVlagaMin = row.GetValue<double>("granica_vlaga_min"),
                GranicaCo2Max = row.GetValue<double>("granica_co2_max"),
                GranicaCo2Min = row.GetValue<double>("granica_co2_min"),
                GranicaSvetlostMax = row.GetValue<double>("granica_svetlost_max"),
                GranicaSvetlostMin = row.GetValue<double>("granica_svetlost_min")
            };
        }

        public static void DodajProizvodnuJedinicu(ProizvodnaJedinica pj)
        {
            Cassandra.ISession session = SessionManager.GetSession();
            var statement = new SimpleStatement(
                "INSERT INTO proizvodne_jedinice (id_jedinice, naziv, geo_lat, geo_long, tip_jedinice, povrsina, status, aktivno, vrsta_biljaka, broj_senzora, status_mreze, poslednji_update, datum_postavljanja, opis, odgovorno_lice, nadmorska_visina, granica_temp_max, granica_temp_min, granica_vlaga_max, granica_vlaga_min, granica_co2_max, granica_co2_min, granica_svetlost_max, granica_svetlost_min) " +
                "VALUES (' pj.Naziv', 'pj.GeoLat', 'pj.GeoLong', 'pj.TipJedinice', 'pj.Povrsina', 'pj.Status', 'pj.Aktivno', 'pj.VrstaBiljaka', 'pj.BrojSenzora', 'pj.StatusMreze', 'pj.PoslednjiUpdate', 'pj.DatumPostavljanja', 'pj.Opis', 'pj.OdgovornoLice', 'pj.NadmorskaVisina', 'pj.GranicaTempMax', 'pj.GranicaTempMin', 'pj.GranicaVlagaMax', 'pj.GranicaVlagaMin', 'pj.GranicaCo2Max', 'pj.GranicaCo2Min', 'pj.GranicaSvetlostMax', 'pj.GranicaSvetlostMin')");
        }

        public static void ObrisiProizvodnuJedinicu(Guid id_jedinice)
        {
            Cassandra.ISession session = SessionManager.GetSession();
            ProizvodnaJedinica pj = new ProizvodnaJedinica();

            if (session == null)
                return; 
            RowSet pjData = session.Execute("delete from \"proizvodne_jedinice\" where \"id_jedinice\" = '" + id_jedinice + "'");
        }

        public static List<ProizvodnaJedinica>? VratiSveProizvodneJedinice()
        {
            Cassandra.ISession session = SessionManager.GetSession();
            List<ProizvodnaJedinica> listaJedinica = new List<ProizvodnaJedinica>();

            if (session == null)
                return null;
            var ProizvodneJedinicaData = session.Execute("select * from \"proizvodne_jedinice\"");


            foreach(var pjData in ProizvodneJedinicaData)
            {
                ProizvodnaJedinica pj = new ProizvodnaJedinica();
                pj.IdJedinice = pjData["id_jedinice"] != null ? (Guid)pjData["id_jedinice"] : Guid.Empty;
                listaJedinica.Add(pj);
            }
            return listaJedinica;
        }

    }
}
