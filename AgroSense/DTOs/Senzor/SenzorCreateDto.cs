using System.ComponentModel.DataAnnotations;

namespace AgroSense.DTOs.Senzor
{
    public class SenzorCreateDto
    {
        [Required]
        public Guid LokacijaId { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Naziv senzora mora imati izmedju 3 i 30 karaktera")]
        public string Naziv { get; set; } = string.Empty;
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Proizvodjac senzora mora imati izmedju 3 i 30 karaktera")]
        public string Proizvodjac { get; set; } = string.Empty;
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Model senzora mora imati izmedju 3 i 30 karaktera")]
        public string Model { get; set; } = string.Empty;
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Status senzora mora imati izmedju 3 i 10 karaktera")]
        public string Status { get; set; } = string.Empty;
        public DateTime VremeInstalacije { get; set; }
    }
}
