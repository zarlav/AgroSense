namespace AgroSense.DTOs.Korisnik
{
    public class KorisnikViewDto
    {
        public Guid Id_korisnika { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Uloga { get; set; } = string.Empty;
        public bool Aktivan { get; set; }
        public DateTime Datum_registracije { get; set; }
    }
}
