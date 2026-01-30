using Cassandra;
using AgroSense.Domain;

public class AlarmRepository
{
    private readonly Cassandra.ISession _session;

    public AlarmRepository(Cassandra.ISession session)
    {
        _session = session;
    }

    public List<Alarm> VratiAlarmePoJedinici(
        Guid idJedinice,
        LocalDate dan,
        DateTime vremeOd,
        DateTime vremeDo)
    {
        var query = new SimpleStatement(
            @"SELECT *
              FROM agrosense.alarmi
              WHERE id_jedinice = ?
                AND dan = ?
                AND vreme_dogadjaja >= ?
                AND vreme_dogadjaja <= ?",
            idJedinice,
            dan,
            vremeOd,
            vremeDo
        );

        var result = _session.Execute(query);
        var alarmi = new List<Alarm>();

        foreach (var row in result)
        {
            alarmi.Add(new Alarm
            {
                IdAlarma = row.GetValue<Guid>("id_alarma"),
                IdJedinice = row.GetValue<Guid>("id_jedinice"),
                TipSenzora = row.GetValue<string>("tip_senzora"),
                Parametar = row.GetValue<string>("parametar"),
                TrenutnaVrednost = row.GetValue<double>("trenutna_vrednost"),
                GranicnaVrednost = row.GetValue<double>("granicna_vrednost"),
                StanjePre = row.GetValue<string>("stanje_pre"),
                StanjePosle = row.GetValue<string>("stanje_posle"),
                Prioritet = row.GetValue<string>("prioritet"),
                VremeDogadjaja = row.GetValue<DateTime>("vreme_dogadjaja"),
                Komentar = row.GetValue<string>("komentar")
            });
        }

        return alarmi;
    }
}
