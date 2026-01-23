using System;

namespace AgroSense.DTOs.ProizvodnaJedinica
{
    public class ProizvodnaJedinicaCreateDto
    {
        public string Tip_jedinice { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public double Geo_lat { get; set; }
        public double Geo_long { get; set; }
        public float Povrsina { get; set; }
        public double Nadmorska_visina { get; set; }
        public string Vrsta_biljaka { get; set; } = string.Empty;
        public string Status_mreze { get; set; } = string.Empty;
        public string Odgovorno_lice { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public bool Aktivno { get; set; } = true;
        public DateTime Datum_postavljanja { get; set; } = DateTime.UtcNow;
        public float Granica_temp_min { get; set; }
        public float Granica_temp_max { get; set; }
        public float Granica_vlaznost_min { get; set; }
        public float Granica_vlaznost_max { get; set; }
        public float Granica_vlaznost_lista_min { get; set; }
        public float Granica_vlaznost_lista_max { get; set; }
        public float Granica_co2_min { get; set; }
        public float Granica_co2_max { get; set; }
        public float Granica_jacina_vetra_min { get; set; }
        public float Granica_jacina_vetra_max { get; set; }
        public float Granica_pritisak_vazduha_min { get; set; }
        public float Granica_pritisak_vazduha_max { get; set; }

        public float Granica_pritisak_u_cevima_min { get; set; }
        public float Granica_pritisak_u_cevima_max { get; set; }
        public float Granica_svetlost_min { get; set; }
        public float Granica_svetlost_max { get; set; }
    }
}