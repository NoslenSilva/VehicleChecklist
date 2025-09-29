using Microsoft.EntityFrameworkCore;

using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;
using VehicleChecklist.Infrastructure.Data;

using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Infrastructure.Repositories
{
    public class ChecklistRepository : IChecklistRepository
    {
        private readonly AppDbContext _db;
        public ChecklistRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Checklist checklist)
        {
            await _db.Checklists.AddAsync(checklist);
        }

        public async Task<Checklist?> GetByIdAsync(Guid id)
        {
            return await _db.Checklists
                .Include(c => c.Vehicle)
                .Include(c => c.StartedBy)
                .Include(c => c.ReviewedBy)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Checklist?> GetInProgressByVehicleAsync(Guid vehicleId)
        {
            return await _db.Checklists
                .Include(c => c.Items)
                .Include(c => c.Vehicle)
                .Include(c => c.StartedBy)
                .FirstOrDefaultAsync(c =>
                    c.VehicleId == vehicleId &&
                    c.Status == Domain.Enums.ChecklistStatus.InProgress);
        }

        public async Task UpdateAsync(Checklist checklist)
        {
            _db.Checklists.Update(checklist);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<List<Checklist>> GetAllAsync(Guid? vehicleId = null, Guid? executorId = null, ChecklistStatus? status = null)
        {
            var query = _db.Checklists
                .Include(c => c.Vehicle)
                .Include(c => c.StartedBy)
                .Include(c => c.ReviewedBy)
                .Include(c => c.Items)
                .AsQueryable();

            if (vehicleId.HasValue)
                query = query.Where(c => c.VehicleId == vehicleId.Value);

            if (executorId.HasValue)
                query = query.Where(c => c.StartedById == executorId.Value);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            return await query.ToListAsync();
        }
    }
}
