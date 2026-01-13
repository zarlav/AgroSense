namespace AgroSense.Domain
{
  public class Alarm
{
    public Guid IdJedinice { get; set; }
    public DateTime Dan { get; set; }
    public DateTime VremeDogadjaja { get; set; }

    public Guid IdAlarma { get; set; }
    public Guid IdSenzora { get; set; }

    public string? TipSenzora { get; set; }
    public string? Parametar { get; set; }

    public double TrenutnaVrednost { get; set; }
    public double GranicnaVrednost { get; set; }

    public string? StanjePre { get; set; }
    public string? StanjePosle { get; set; }

    public string? Prioritet { get; set; }
    public string? Komentar { get; set; }

    public DateTime VremeAktivacije { get; set; }
    public bool AutomatizovanaAkcija { get; set; }
}

}
