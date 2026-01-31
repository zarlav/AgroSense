using System.ComponentModel.DataAnnotations;

namespace AgroSense.DTOs.ProizvodnaJedinica
{
    public class ProizvodnaJedinicaUpdateDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Naziv jedinice mora biti izmedju 3 i 30 karaktera!")]
        public string Naziv { get; set; } = string.Empty;
        [Required]
        public double Geo_lat { get; set; }
        [Required]
        public double Geo_long { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Tip jedinice mora biti izmedju 3 i 30 karaktera!")]
        public string Tip_jedinice { get; set; } = string.Empty;
        [Required]
        [Range(0.1, 100000, ErrorMessage = "Povrsina mora biti pozitivna")]
        public float Povrsina { get; set; }
        [Required]
        [Range(-500, 9000, ErrorMessage = "Nadmorska visina je van opsega (-500, 9000).")]
        public double Nadmorska_visina { get; set; }
        [Required]
        [StringLength(50)]
        public string Vrsta_biljaka { get; set; } = string.Empty;
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Status_mreze { get; set; } = string.Empty;
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Odgovorno_lice { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Opis { get; set; } = string.Empty;
        [Required]
        public bool Aktivno { get; set; }
        [Required]
        public float Granica_temp_min { get; set; }
        [Required]
        public float Granica_temp_max { get; set; }
        [Required]
        public float Granica_vlaznost_min { get; set; }
        [Required]
        public float Granica_vlaznost_max { get; set; }
        [Required]
        public float Granica_vlaznost_lista_min { get; set; }
        [Required]
        public float Granica_vlaznost_lista_max { get; set; }
        [Required]
        public float Granica_co2_min { get; set; }
        [Required]
        public float Granica_co2_max { get; set; }
        [Required]
        public float Granica_jacina_vetra_min { get; set; }
        [Required]
        public float Granica_jacina_vetra_max { get; set; }
        [Required]
        public float Granica_pritisak_u_cevima_min { get; set; }
        [Required]
        public float Granica_pritisak_u_cevima_max { get; set; }
        [Required]
        public float Granica_pritisak_vazduha_min { get; set; }
        [Required]
        public float Granica_pritisak_vazduha_max { get; set; }
        [Required]
        public float Granica_svetlost_min { get; set; }
        [Required]
        public float Granica_svetlost_max { get; set; }
    }
}