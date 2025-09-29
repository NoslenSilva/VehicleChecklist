using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;

namespace VehicleChecklist.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected User GetUserFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            if (string.IsNullOrEmpty(idClaim) || string.IsNullOrEmpty(roleClaim))
                throw new UnauthorizedAccessException("Usuário não autenticado corretamente.");

            return new User
            {
                Id = Guid.Parse(idClaim),
                FullName = User.Identity?.Name ?? "Authenticated User",
                Email = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? "unknown",
                Role = Enum.Parse<UserRole>(roleClaim)
            };
        }
    }
}
