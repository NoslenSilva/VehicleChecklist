using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Application.Services;
using VehicleChecklist.Domain.Enums;

namespace VehicleChecklist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")] // apenas admin pode criar novos usuários
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var user = await _service.RegisterAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _service.LoginAsync(dto);
            return Ok(new { Token = token });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpPost("seed-admin")]
        [AllowAnonymous] // permite rodar sem autenticação
        public async Task<IActionResult> SeedAdmin()
        {
            var user = await _service.SeedAdminAsync();
            return Ok(user);
        }
    }
}
