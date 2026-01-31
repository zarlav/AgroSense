using Cassandra;
using System.ComponentModel.DataAnnotations;

namespace AgroSense.DTOs.Merenje
{
    public class MerenjeCreateDto
    {
        [Required]
        public Guid Id_senzora { get; set; }
        [Required]
        public Guid Id_loakcije { get; set; }
        [Required]
        public required LocalDate Datum { get; set; }
        [Required]
        public DateTime Ts { get; set; }
        [Required]
        [Range(-30, 42, ErrorMessage = "Temperatura mora biti u opsegu -30 - 42")]
        public float Temperatura { get; set; }
        [Required]
        [Range(0,100, ErrorMessage = "Vlaznost mora biti u opsegu 0-100")]
        public float Vlaznost { get; set; }
        [Required]
        [Range(380,460, ErrorMessage ="CO2 mora biti u opsegu 380-460")]
        public float Co2 { get; set; }
        [Required]
        [Range(0,10, ErrorMessage ="Jacina vetra mora biti u opsegu 0-10")]
        public float Jacina_vetra { get; set; }
        [Required]
        [Range(6.0, 7.0, ErrorMessage ="Vrednost ph zemlje mora biti u opsegu 6-7")]
        public float Ph_zemljista { get; set; }
        [Required]
        [StringLength(3, MinimumLength =1)]
        public string smer_vetra { get; set; } = string.Empty;
        [Required]
        [Range(0,10, ErrorMessage ="UV mora biti u opsegu 0-10")]
        public float Uv_vrednost { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Vlaznost mora biti u opsegu 0-100")]
        public float Vlaznost_lista { get; set; }
        [Required]
        [Range(960,1060,ErrorMessage ="Pritisak vazduha mora biti u opsegu 960-1060")]
        public float Pritisak_vazduha { get; set; }
        [Required]
        [Range(2.0, 5.0, ErrorMessage = "Pritisak u cevima mora biti u opsegu 2.0-5.0")]
        public float Pritisak_u_cevima { get; set; }
    }
}
