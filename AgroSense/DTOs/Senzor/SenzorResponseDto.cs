namespace AgroSense.DTOs.Senzor
{
    public class SenzorResponseDto
    {
        public string Naziv { get; set; } = string.Empty;
        public string Proizvodjac { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime VremeInstalacije { get; set; }
    }
}
