namespace AgroSense.DTOs.Korisnik
{
    public class KorisnikCreateDto
    {
        public string Ime { get; set; } = string.Empty;
        public string Kontakt { get; set; } = string.Empty;
        public string Uloga { get; set; } = string.Empty;
        public bool Aktivan { get; set; }
    }
}
