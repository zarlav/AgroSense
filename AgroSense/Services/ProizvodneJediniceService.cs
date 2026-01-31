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

        public async Task Dodaj(ProizvodnaJedinicaCreateDto dto)
        {
            await _repo.Dodaj(dto);
        }

        public async Task<List<ProizvodnaJedinicaResponseDto>> VratiSve()
        {
            return await _repo.VratiSve();
        }

        public async Task<ProizvodnaJedinicaViewDto?> VratiPoId(string tip,bool stanje, Guid id)
        {
            return await _repo.VratiPoId(tip, stanje, id);
        }

        public async Task Update(string tip, bool stanje, Guid id, ProizvodnaJedinicaUpdateDto dto)
        {
            await _repo.Update(tip, stanje, id, dto);
        }

        public async Task Obrisi(string tip, bool stanje, Guid id)
        {
            await _repo.Obrisi(tip, stanje, id);
        }

        public async Task<List<Guid>> VratiSveIdjeve()
        {
            return await _repo.VratiSveIdjeve();
        }
    }
}