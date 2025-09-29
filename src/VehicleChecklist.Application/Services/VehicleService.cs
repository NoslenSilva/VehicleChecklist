using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Application.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository _repo;

        public VehicleService(IVehicleRepository repo)
        {
            _repo = repo;
        }

        public async Task<VehicleResponseDto> CreateAsync(CreateVehicleDto dto)
        {
            var existing = await _repo.GetByPlateAsync(dto.Plate);
            if (existing != null) throw new InvalidOperationException("Placa já cadastrada.");

            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Plate = dto.Plate.ToUpper(),
                Model = dto.Model,
                Notes = dto.Notes
            };

            await _repo.AddAsync(vehicle);
            await _repo.SaveChangesAsync();

            return new VehicleResponseDto(vehicle.Id, vehicle.Plate, vehicle.Model, vehicle.Notes);
        }

        public async Task<VehicleResponseDto?> GetByIdAsync(Guid id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            return vehicle == null ? null : new VehicleResponseDto(vehicle.Id, vehicle.Plate, vehicle.Model, vehicle.Notes);
        }

        public async Task<List<VehicleResponseDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(v => new VehicleResponseDto(v.Id, v.Plate, v.Model, v.Notes)).ToList();
        }

        public async Task<VehicleResponseDto> UpdateAsync(Guid id, UpdateVehicleDto dto)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null) throw new KeyNotFoundException("Veículo não encontrado.");

            vehicle.Model = dto.Model;
            vehicle.Notes = dto.Notes;

            await _repo.UpdateAsync(vehicle);
            await _repo.SaveChangesAsync();

            return new VehicleResponseDto(vehicle.Id, vehicle.Plate, vehicle.Model, vehicle.Notes);
        }

        public async Task DeleteAsync(Guid id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null) throw new KeyNotFoundException("Veículo não encontrado.");

            await _repo.DeleteAsync(vehicle);
            await _repo.SaveChangesAsync();
        }

        public async Task<VehicleResponseDto?> GetByPlateAsync(string plate)
        {
            var vehicle = await _repo.GetByPlateAsync(plate.ToUpper());
            return vehicle == null
                ? null
                : new VehicleResponseDto(vehicle.Id, vehicle.Plate, vehicle.Model, vehicle.Notes);
        }
    }
}
