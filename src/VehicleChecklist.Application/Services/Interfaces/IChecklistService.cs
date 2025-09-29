using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;

namespace VehicleChecklist.Application.Services.Interfaces
{
    public interface IChecklistService
    {
        Task<Checklist> StartChecklistAsync(Guid vehicleId, User executor);
        Task<Checklist> StartChecklistByPlateAsync(string plate, User executor);
        Task<Checklist> AddItemAsync(Guid checklistId, string name, User executor);
        Task<Checklist> FinishChecklistAsync(Guid checklistId, User executor);
        Task<Checklist> ReviewChecklistAsync(Guid checklistId, bool approve, User supervisor);
        Task<Checklist> UpdateItemAsync(Guid checklistId, Guid itemId, bool? isOk, string? observation, User executor);
        Task<Checklist> RemoveItemAsync(Guid checklistId, Guid itemId, User executor);
        Task<Checklist?> GetByIdAsync(Guid id);
        Task<List<Checklist>> GetAllAsync(Guid? vehicleId = null, Guid? executorId = null, ChecklistStatus? status = null);
    }
}
