using System;
using Cassandra;
using System.ComponentModel.DataAnnotations;
namespace AgroSense.DTOs.Alarm
{
    public class AlarmCreateDTO
    {
        [Required]
        public TimeUuid Id_alarm { get; set; }
        [Required]
        public Guid Id_jedinice { get; set; }
        [Required]
        public Guid Id_senzora { get; set; }
        [Required]
        public required LocalDate Dan { get; set; }    
        [Required]
        public DateTime Vreme_dogadjaja { get; set; }
        [Required]
        public string Parametar { get; set; } = string.Empty;
        [Required]
        public double Trenutna_vrednost { get; set; }
        [Required]
        public double Granicna_vrednostMin { get; set; }
        [Required]
        public double Granicna_vrednostMax { get; set; }
        [Required]
        public string Komentar { get; set; } = string.Empty;
    }
}
