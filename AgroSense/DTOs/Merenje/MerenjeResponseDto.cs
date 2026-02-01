using Cassandra;

namespace AgroSense.DTOs.Merenje
{
    public class MerenjeResponseDto
    {
        public Guid Id_senzora { get; set; }
        public required LocalDate Datum { get; set; }
        public DateTime Ts { get; set; }
        public float Temperatura { get; set; }
        public float Vlaznost { get; set; }
        public float Co2 { get; set; }
        public float Jacina_vetra { get; set; }
        public float Ph_zemljista { get; set; }
        public string smer_vetra { get; set; } = string.Empty;
        public float Uv_vrednost { get; set; }
        public float Vlaznost_lista { get; set; }
        public float Pritisak_vazduha { get; set; }
        public float Pritisak_u_cevima { get; set; }
    }
}
