using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Application.Services.Interfaces;
using VehicleChecklist.Domain.Enums;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChecklistsController : BaseController
    {
        private readonly IChecklistService _service;

        public ChecklistsController(IChecklistService service, IUserRepository userRepository) : base(userRepository)
        {
            _service = service;
        }

        /// <summary>
        /// Inicia um checklist por VehicleId.
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartChecklistDto dto)
        {
            var user = await GetUserFromClaimsAsync();
            if (user.Role != UserRole.Executor && user.Role != UserRole.Admin) return Forbid();

            var checklist = await _service.StartChecklistAsync(dto.VehicleId, user);
            return CreatedAtAction(nameof(GetById), new { id = checklist.Id }, checklist);
        }

        /// <summary>
        /// Inicia um checklist por placa do veículo.
        /// </summary>
        [HttpPost("start-by-plate")]
        public async Task<IActionResult> StartByPlate([FromBody] StartChecklistByPlateDto dto)
        {
            var user = await GetUserFromClaimsAsync();
            if (user.Role != UserRole.Executor && user.Role != UserRole.Admin) return Forbid();

            var checklist = await _service.StartChecklistByPlateAsync(dto.Plate, user);
            return CreatedAtAction(nameof(GetById), new { id = checklist.Id }, checklist);
        }

        /// <summary>
        /// Adiciona um item a um checklist em progresso.
        /// </summary>
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody] AddItemDto dto)
        {
            var user = await GetUserFromClaimsAsync();
            var checklist = await _service.AddItemAsync(dto.ChecklistId, dto.Name, user);
            return Ok(checklist);
        }

        /// <summary>
        /// Atualiza um item de checklist (status e observação).
        /// </summary>
        [HttpPut("{id:guid}/items/{itemId:guid}")]
        public async Task<IActionResult> UpdateItem(Guid id, Guid itemId, [FromBody] UpdateChecklistItemDto dto)
        {
            var user = await GetUserFromClaimsAsync();
            var checklist = await _service.UpdateItemAsync(id, itemId, dto.IsOk, dto.Observation, user);
            return Ok(checklist);
        }

        /// <summary>
        /// Remove um item de checklist em progresso.
        /// </summary>
        [HttpDelete("{id:guid}/items/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid id, Guid itemId)
        {
            var user = await GetUserFromClaimsAsync();
            var checklist = await _service.RemoveItemAsync(id, itemId, user);
            return Ok(checklist);
        }

        /// <summary>
        /// Finaliza um checklist em progresso (somente executor que iniciou).
        /// </summary>
        [HttpPost("finish/{id:guid}")]
        public async Task<IActionResult> Finish(Guid id)
        {
            var user = await GetUserFromClaimsAsync();
            var checklist = await _service.FinishChecklistAsync(id, user);
            return Ok(checklist);
        }

        /// <summary>
        /// Revisão do checklist (aprovação ou reprovação).
        /// </summary>
        [HttpPost("review/{id:guid}")]
        public async Task<IActionResult> Review(Guid id, [FromQuery] bool approve)
        {
            var user = await GetUserFromClaimsAsync();
            if (user.Role != UserRole.Supervisor && user.Role != UserRole.Admin) return Forbid();

            var checklist = await _service.ReviewChecklistAsync(id, approve, user);
            return Ok(checklist);
        }

        /// <summary>
        /// Busca checklist por Id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var checklist = await _service.GetByIdAsync(id);
            if (checklist == null) return NotFound();
            return Ok(checklist);
        }

        /// <summary>
        /// Lista todos os checklists, com filtros opcionais.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Supervisor,Admin")]
        public async Task<IActionResult> GetAll([FromQuery] Guid? vehicleId, [FromQuery] Guid? executorId, [FromQuery] ChecklistStatus? status)
        {
            var checklists = await _service.GetAllAsync(vehicleId, executorId, status);
            return Ok(checklists);
        }
    }
}