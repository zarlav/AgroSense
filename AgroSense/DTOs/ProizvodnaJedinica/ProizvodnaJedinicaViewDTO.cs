using System;

namespace AgroSense.DTOs.ProizvodnaJedinica
{
    public class ProizvodnaJedinicaViewDto : ProizvodnaJedinicaCreateDto
    {
        public Guid Id_jedinice { get; set; }
    }
}