using AgroSense.DTOs.Alarm;
using Cassandra;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class AlarmRepository
{
    private readonly Cassandra.ISession _session;
    private readonly PreparedStatement _psKreirajAlarm;

    public AlarmRepository(Cassandra.ISession session)
    {
        _session = session;
        _psKreirajAlarm = _session.Prepare(@"
        INSERT INTO alarmi 
        (id_jedinice, dan, vreme_dogadjaja, id_alarma, id_senzora, parametar, trenutna_vrednost, granicna_vrednostmin,
         granicna_vrednostmax, komentar)
        VALUES (?,?,?,?,?,?,?,?,?,?)");
    }

    public async Task KreirajAlarm(AlarmCreateDTO adto)
    {
        var id = TimeUuid.NewId();

        var bound = _psKreirajAlarm.Bind(
            adto.Id_jedinice,
            adto.Dan,
            adto.Vreme_dogadjaja,
            id,
            adto.Id_senzora,
            adto.Parametar,
            adto.Trenutna_vrednost,
            adto.Granicna_vrednostMin,
            adto.Granicna_vrednostMax,
            adto.Komentar
            );

        await _session.ExecuteAsync(bound).ConfigureAwait(false);
    }

    public async Task<List<AlarmResponseDTO>> VratiAlarmePoJedinici(
        Guid idJedinice,
        LocalDate dan,
        TimeSpan vremeOd,
        TimeSpan vremeDo)
    {
        DateTime osnovniDatum = new DateTime(dan.Year, dan.Month, dan.Day);
        DateTime pocetak = osnovniDatum.Add(vremeOd);
        DateTime kraj = osnovniDatum.Add(vremeDo);
        var ps = _session.Prepare(
            @"SELECT *
              FROM agrosense.alarmi
              WHERE id_jedinice = ?
                AND dan = ?
                AND vreme_dogadjaja >= ?
                AND vreme_dogadjaja <= ?"
        );

        var rs = await _session.ExecuteAsync(ps.Bind(idJedinice,dan,pocetak, kraj));

        return rs.Select(row => new AlarmResponseDTO
        {
            Id_jedinice = row.GetValue<Guid>("id_jedinice"),
            Dan = row.GetValue<LocalDate>("dan"),
            Vreme_dogadjaja = row.GetValue<DateTime>("vreme_dogadjaja"),
            Id_senzora = row.GetValue<Guid>("id_senzora"),
            Parametar = row.GetValue<string>("parametar"),
            Trenutna_vrednost = row.GetValue<double>("trenutna_vrednost"),
            Granicna_vrednostMin = row.GetValue<double>("granicna_vrednostmin"),
            Granicna_vrednostMax = row.GetValue<double>("granicna_vrednostmax"),
            Komentar = row.GetValue<string>("komentar")
        }).ToList();
    }
}
