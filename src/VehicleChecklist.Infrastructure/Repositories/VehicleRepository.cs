using Microsoft.EntityFrameworkCore;

using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Infrastructure.Data;

using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _db;
        public VehicleRepository(AppDbContext db) => _db = db;

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle?> GetByPlateAsync(string plate)
        {
            return await _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _db.Vehicles.ToListAsync();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _db.Vehicles.AddAsync(vehicle);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _db.Vehicles.Update(vehicle);
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {
            _db.Vehicles.Remove(vehicle);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
