using Microsoft.AspNetCore.Mvc;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        protected BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Obtém o usuário autenticado a partir do token JWT consultando no banco de dados.
        /// </summary>
        protected async Task<User> GetUserFromClaimsAsync()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(idClaim))
                throw new UnauthorizedAccessException("Usuário não autenticado corretamente.");

            var user = await _userRepository.GetByIdAsync(Guid.Parse(idClaim));
            if (user == null)
                throw new UnauthorizedAccessException("Usuário não encontrado no banco.");

            return user;
        }
    }
}
