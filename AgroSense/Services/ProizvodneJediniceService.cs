using System;
using System.Collections.Generic; // Dodato za podršku Listi
using AgroSense.DTOs.ProizvodnaJedinica;
using AgroSense.Repositories.ProizvodneJedinice;

namespace AgroSense.Services
{
    public class ProizvodneJediniceService
    {
        private readonly ProizvodneJediniceRepository _repo;

        public ProizvodneJediniceService(ProizvodneJediniceRepository repo)
        {
            _repo = repo;
        }

        public void Dodaj(ProizvodnaJedinicaCreateDto dto)
        {
            _repo.Dodaj(dto);
        }

        public List<ProizvodnaJedinicaResponseDto> VratiSve()
        {
            return _repo.VratiSve();
        }

        public ProizvodnaJedinicaViewDto? VratiPoId(string tip, Guid id)
        {
            return _repo.VratiPoId(tip, id);
        }

        public void Update(string tip, Guid id, ProizvodnaJedinicaUpdateDto dto)
        {
            _repo.Update(tip, id, dto);
        }

        public void Obrisi(string tip, Guid id)
        {
            _repo.Obrisi(tip, id);
        }

        public List<Guid> VratiSveIdjeve()
        {
            return _repo.VratiSveIdjeve();
        }
    }
}