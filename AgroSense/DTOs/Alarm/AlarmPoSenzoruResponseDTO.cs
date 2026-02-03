using Cassandra;

namespace AgroSense.DTOs.Alarm
{
    public class AlarmPoSenzoruResponseDTO
    {
        public Guid Id_jedinice { get; set; }
        public required LocalDate Dan { get; set; }
        public DateTime Vreme_dogadjaja { get; set; }
        public string Komentar { get; set; } = string.Empty;
    }
}
