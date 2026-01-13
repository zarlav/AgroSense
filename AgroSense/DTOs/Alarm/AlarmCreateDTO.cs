using System;

namespace AgroSense.DTOs.Alarm
{
    public class AlarmCreateDTO
    {
        public Guid Id_jedinice { get; set; }
        public Guid Id_senzora { get; set; }
        public DateOnly Dan { get; set; }          
        public DateTime Vreme_dogadjaja { get; set; } 
        public string Tip_senzora { get; set; } = string.Empty;
        public string Parametar { get; set; } = string.Empty;
        public double Trenutna_vrednost { get; set; }
        public double Granicna_vrednost { get; set; }
        public string Stanje_pre { get; set; } = string.Empty;
        public string Stanje_posle { get; set; } = string.Empty;
        public string Prioritet { get; set; } = string.Empty;
        public string Komentar { get; set; } = string.Empty;
        public DateTime Vreme_aktivacije { get; set; }
        public bool Automatizovana_akcija { get; set; }
    }
}
