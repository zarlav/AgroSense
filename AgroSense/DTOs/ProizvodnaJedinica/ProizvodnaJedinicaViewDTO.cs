using System;

namespace AgroSense.DTOs.ProizvodnaJedinica
{
    public class ProizvodnaJedinicaViewDto : ProizvodnaJedinicaCreateDto
    {
        // Pored svih polja koja nasleđuje iz CreateDto, 
        // ViewDto mora da sadrži i primarni ključ iz baze.
        public Guid Id_jedinice { get; set; }
    }
}