using Cassandra;
using System.ComponentModel.DataAnnotations;

public class AlarmResponseDTO
{
    public Guid Id_jedinice { get; set; }
    public Guid Id_senzora { get; set; }
    public required LocalDate Dan { get; set; }
    public DateTime Vreme_dogadjaja { get; set; }
    public string Parametar { get; set; } = string.Empty;
    public double Trenutna_vrednost { get; set; }
    public double Granicna_vrednostMin { get; set; }
    public double Granicna_vrednostMax { get; set; }
    public string Komentar { get; set; } = string.Empty;

}
