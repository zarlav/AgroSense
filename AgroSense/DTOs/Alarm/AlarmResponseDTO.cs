using System;
using Cassandra;

namespace AgroSense.DTOs.Alarm
{
    public class AlarmResponseDTO
    {
        public Guid LokacijaId { get; set; }
        public Guid SenzorId { get; set; }

        public string Parametar { get; set; }
        public float Vrednost { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }

        public DateTime Vreme { get; set; }
        public string Poruka { get; set; }
    }
}
