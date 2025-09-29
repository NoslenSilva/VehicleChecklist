using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;

namespace VehicleChecklist.Infrastructure.Repositories.Interfaces
{
    public interface IChecklistRepository
    {
        Task<Checklist?> GetByIdAsync(Guid id);
        Task AddAsync(Checklist checklist);
        Task UpdateAsync(Checklist checklist);
        Task SaveChangesAsync();
        Task<Checklist?> GetInProgressByVehicleAsync(Guid vehicleId);
        Task<List<Checklist>> GetAllAsync(Guid? vehicleId = null, Guid? executorId = null, ChecklistStatus? status = null);
    }
}
