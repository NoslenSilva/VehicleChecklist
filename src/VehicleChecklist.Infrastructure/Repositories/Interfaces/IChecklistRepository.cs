using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VehicleChecklist.Domain.Entities;

namespace VehicleChecklist.Infrastructure.Repositories.Interfaces
{
    public interface IChecklistRepository
    {
        Task<Checklist?> GetByIdAsync(Guid id);
        Task AddAsync(Checklist checklist);
        Task UpdateAsync(Checklist checklist);
        Task SaveChangesAsync();
        Task<Checklist?> GetInProgressByVehicleAsync(Guid vehicleId);
    }
}
