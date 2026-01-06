namespace AgroSense.Domain
{
    public class Korisnik
    {
        public Guid Id_korisnika { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Kontakt { get; set; } = string.Empty;
        public string Uloga { get; set; } = string.Empty;
        public bool Aktivan { get; set; }
    }
}
