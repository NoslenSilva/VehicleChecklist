using VehicleChecklist.Domain.Entities;

namespace VehicleChecklist.Infrastructure.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<Vehicle?> GetByPlateAsync(string plate);
        Task<List<Vehicle>> GetAllAsync();
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
        Task SaveChangesAsync();

    }
}
