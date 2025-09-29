using VehicleChecklist.Application.DTOs;

namespace VehicleChecklist.Application.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleResponseDto> CreateAsync(CreateVehicleDto dto);
        Task<VehicleResponseDto?> GetByIdAsync(Guid id);
        Task<List<VehicleResponseDto>> GetAllAsync();
        Task<VehicleResponseDto> UpdateAsync(Guid id, UpdateVehicleDto dto);
        Task DeleteAsync(Guid id);
        Task<VehicleResponseDto?> GetByPlateAsync(string plate);
    }
}
