using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Application.Services
{
    public class ChecklistService
    {
        private readonly IChecklistRepository _repo;
        private readonly IVehicleRepository _vehicleRepo;
        public ChecklistService(IChecklistRepository repo, IVehicleRepository vehicleRepo)
        {
            _repo = repo;
            _vehicleRepo = vehicleRepo;
        }
        public async Task<Checklist> StartChecklistAsync(Guid vehicleId, User executor)
        {
            // check if there's an in-progress for same vehicle
            var existing = await _repo.GetInProgressByVehicleAsync(vehicleId);
            if (existing != null)
                throw new InvalidOperationException("Já existe um checklist em execução para este veículo.");

            var checklist = new Checklist
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicleId,
                StartedById = executor.Id,
                StartedAt = DateTime.UtcNow,
                Locked = true,
                Status = ChecklistStatus.InProgress
            };

            await _repo.AddAsync(checklist);
            await _repo.SaveChangesAsync();
            return checklist;
        }

        public async Task<Checklist> StartChecklistByPlateAsync(string plate, User executor)
        {
            var vehicle = await _vehicleRepo.GetByPlateAsync(plate.ToUpper());
            if (vehicle == null) throw new KeyNotFoundException("Veículo não encontrado.");

            return await StartChecklistAsync(vehicle.Id, executor);
        }

        public async Task<Checklist> AddItemAsync(Guid checklistId, string name, User executor)
        {
            var checklist = await _repo.GetByIdAsync(checklistId);
            if (checklist == null) throw new KeyNotFoundException("Checklist não encontrado.");
            if (checklist.Status != ChecklistStatus.InProgress) throw new InvalidOperationException("Checklist não está em andamento.");
            if (checklist.StartedById != executor.Id) throw new InvalidOperationException("Somente o executor que iniciou pode alterar este checklist enquanto em progresso.");

            checklist.Items.Add(new ChecklistItem { Id = Guid.NewGuid(), Name = name, ChecklistId = checklistId });
            await _repo.UpdateAsync(checklist);
            await _repo.SaveChangesAsync();
            return checklist;
        }

        public async Task<Checklist> FinishChecklistAsync(Guid checklistId, User executor)
        {
            var checklist = await _repo.GetByIdAsync(checklistId);
            if (checklist == null) throw new KeyNotFoundException("Checklist não encontrado.");
            if (checklist.StartedById != executor.Id) throw new InvalidOperationException("Somente o executor que iniciou pode finalizar.");
            checklist.FinishedAt = DateTime.UtcNow;
            checklist.Status = ChecklistStatus.Finished;
            checklist.Locked = false;
            await _repo.UpdateAsync(checklist);
            await _repo.SaveChangesAsync();
            return checklist;
        }

        public async Task<Checklist> ReviewChecklistAsync(Guid checklistId, bool approve, User supervisor)
        {
            var checklist = await _repo.GetByIdAsync(checklistId);
            if (checklist == null) throw new KeyNotFoundException("Checklist não encontrado.");
            if (checklist.Status != ChecklistStatus.Finished) throw new InvalidOperationException("Checklist deve estar finalizado para revisão.");
            if (supervisor == null) throw new ArgumentNullException(nameof(supervisor));
            checklist.ReviewedById = supervisor.Id;
            checklist.Status = approve ? ChecklistStatus.Approved : ChecklistStatus.Rejected;
            await _repo.UpdateAsync(checklist);
            await _repo.SaveChangesAsync();
            return checklist;
        }

        public async Task<Checklist> UpdateItemAsync(Guid checklistId, Guid itemId, bool? isOk, string? observation, User executor)
        {
            var checklist = await _repo.GetByIdAsync(checklistId);
            if (checklist == null) throw new KeyNotFoundException("Checklist não encontrado.");
            if (checklist.Status != ChecklistStatus.InProgress) throw new InvalidOperationException("Checklist não está em andamento.");
            if (checklist.StartedById != executor.Id) throw new InvalidOperationException("Somente o executor que iniciou pode alterar este checklist.");

            var item = checklist.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) throw new KeyNotFoundException("Item não encontrado.");

            item.IsOk = isOk;
            item.Observation = observation;

            await _repo.UpdateAsync(checklist);
            await _repo.SaveChangesAsync();

            return checklist;
        }

        public async Task<Checklist> RemoveItemAsync(Guid checklistId, Guid itemId, User executor)
        {
            var checklist = await _repo.GetByIdAsync(checklistId);
            if (checklist == null) throw new KeyNotFoundException("Checklist não encontrado.");
            if (checklist.Status != ChecklistStatus.InProgress) throw new InvalidOperationException("Checklist não está em andamento.");
            if (checklist.StartedById != executor.Id) throw new InvalidOperationException("Somente o executor que iniciou pode alterar este checklist.");

            var item = checklist.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) throw new KeyNotFoundException("Item não encontrado.");

            checklist.Items.Remove(item);

            await _repo.UpdateAsync(checklist);
            await _repo.SaveChangesAsync();

            return checklist;
        }
    }
}
