namespace AgroSense.DTOs.ProizvodnaJedinica
{
    public class ProizvodnaJedinicaUpdateDto
    {
        public string Naziv { get; set; } = string.Empty;
        public double Geo_lat { get; set; }
        public double Geo_long { get; set; }
        public string Tip_jedinice { get; set; } = string.Empty;
        public float Povrsina { get; set; }

        public float Granica_temp_min { get; set; }
        public float Granica_temp_max { get; set; }
        public float Granica_vlaznost_min { get; set; }
        public float Granica_vlaznost_max { get; set; }

        public string Vrsta_biljaka { get; set; } = string.Empty;
        public string Status_mreze { get; set; } = string.Empty;
        public double Nadmorska_visina { get; set; }
        public string Odgovorno_lice { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public bool Aktivno { get; set; }
    }
}
