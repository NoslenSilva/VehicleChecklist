using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Application.Services.Interfaces;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController : BaseController
    {
        private readonly IVehicleService _service;

        public VehiclesController(IVehicleService service, IUserRepository userRepository) : base(userRepository)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém todos os Veículos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _service.GetAllAsync();
            return Ok(vehicles);
        }

        /// <summary>
        /// Obtém Veículo Específico
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _service.GetByIdAsync(id);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }

        /// <summary>
        /// Inserir Novo Veículo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Create([FromBody] CreateVehicleDto dto)
        {
            var user = await GetUserFromClaimsAsync();
            var vehicle = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        /// <summary>
        /// Atualizar Veículo
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleDto dto)
        {
            var vehicle = await _service.UpdateAsync(id, dto);
            return Ok(vehicle);
        }

        /// <summary>
        /// Excluir Veículo
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Consulta Veículo pela Placa
        /// </summary>
        /// <returns></returns>
        [HttpGet("by-plate/{plate}")]
        public async Task<IActionResult> GetByPlate(string plate)
        {
            var vehicle = await _service.GetByPlateAsync(plate);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }
    }
}
