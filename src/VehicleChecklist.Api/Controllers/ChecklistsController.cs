using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Application.Services;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;

namespace VehicleChecklist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // exige autenticação para todos endpoints
    public class ChecklistsController : BaseController
    {
        private readonly ChecklistService _service;

        public ChecklistsController(ChecklistService service) 
        { 
            _service = service; 
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartChecklistDto dto)
        {
            var user = GetUserFromClaims();
            if (user.Role != UserRole.Executor && user.Role != UserRole.Admin) return Forbid();

            var checklist = await _service.StartChecklistAsync(dto.VehicleId, user);
            return CreatedAtAction(nameof(GetById), new { id = checklist.Id }, checklist);
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody] AddItemDto dto)
        {
            var user = GetUserFromClaims();
            var checklist = await _service.AddItemAsync(dto.ChecklistId, dto.Name, user);
            return Ok(checklist);
        }

        [HttpPost("finish/{id:guid}")]
        public async Task<IActionResult> Finish(Guid id)
        {
            var user = GetUserFromClaims();
            var checklist = await _service.FinishChecklistAsync(id, user);
            return Ok(checklist);
        }

        [HttpPost("review/{id:guid}")]
        public async Task<IActionResult> Review(Guid id, [FromQuery] bool approve)
        {
            var user = GetUserFromClaims();
            if (user.Role != UserRole.Supervisor && user.Role != UserRole.Admin) return Forbid();
            var checklist = await _service.ReviewChecklistAsync(id, approve, user);
            return Ok(checklist);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok();//Results.NotFound();
        }

        [HttpPost("start-by-plate")]
        public async Task<IActionResult> StartByPlate([FromBody] StartChecklistByPlateDto dto)
        {
            var user = GetUserFromClaims();
            if (user.Role != UserRole.Executor && user.Role != UserRole.Admin) return Forbid();

            var checklist = await _service.StartChecklistByPlateAsync(dto.Plate, user);
            return CreatedAtAction(nameof(GetById), new { id = checklist.Id }, checklist);
        }

        [HttpPut("{id:guid}/items/{itemId:guid}")]
        public async Task<IActionResult> UpdateItem(Guid id, Guid itemId, [FromBody] UpdateChecklistItemDto dto)
        {
            var user = GetUserFromClaims();
            var checklist = await _service.UpdateItemAsync(id, itemId, dto.IsOk, dto.Observation, user);
            return Ok(checklist);
        }

        [HttpDelete("{id:guid}/items/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid id, Guid itemId)
        {
            var user = GetUserFromClaims();
            var checklist = await _service.RemoveItemAsync(id, itemId, user);
            return Ok(checklist);
        }
    }
}
