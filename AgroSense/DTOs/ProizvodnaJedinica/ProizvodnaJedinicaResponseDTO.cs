using System;

namespace AgroSense.DTOs.ProizvodnaJedinica
{
    public class ProizvodnaJedinicaResponseDto
    {
        public string Tip_jedinice { get; set; } = string.Empty;
        public Guid Id_jedinice { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public float Povrsina { get; set; }
        public bool Aktivno { get; set; }

        public string Vrsta_biljaka { get; set; } = string.Empty;
        public string Odgovorno_lice { get; set; } = string.Empty;
    }
}