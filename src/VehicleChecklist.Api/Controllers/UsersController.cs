using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Application.Services.Interfaces;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;

        public UsersController(IUserService service, IUserRepository userRepository) : base(userRepository)
        {
            _service = service;
        }

        /// <summary>
        /// Registra Usuário
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var admin = await GetUserFromClaimsAsync();
            var user = await _service.RegisterAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
        }

        /// <summary>
        /// Login do Usuário
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _service.LoginAsync(dto);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Retorna Todos os Usuários
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Criação de usuário Admin para Iniciar o Sistema
        /// </summary>
        /// <returns></returns>
        [HttpPost("seed-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedAdmin()
        {
            var user = await _service.SeedAdminAsync();
            return Ok(user);
        }
    }
}
