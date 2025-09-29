using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using VehicleChecklist.Application.Services.Interfaces;

namespace VehicleChecklist.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterUserDto dto)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null) throw new InvalidOperationException("E-mail já cadastrado.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return new UserResponseDto(user.Id, user.FullName, user.Email, user.Role);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null) throw new UnauthorizedAccessException("Usuário não encontrado.");
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Senha inválida.");

            return GenerateJwt(user);
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return users.Select(u => new UserResponseDto(u.Id, u.FullName, u.Email, u.Role)).ToList();
        }

        private string GenerateJwt(User user)
        {
            var secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret não configurado");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("role", user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResponseDto> SeedAdminAsync()
        {
            var existing = await _repo.GetByEmailAsync("admin@teste.com");
            if (existing != null)
            {
                return new UserResponseDto(existing.Id, existing.FullName, existing.Email, existing.Role);
            }

            var admin = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Administrador",
                Email = "admin@teste.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = Domain.Enums.UserRole.Admin
            };

            await _repo.AddAsync(admin);
            await _repo.SaveChangesAsync();

            return new UserResponseDto(admin.Id, admin.FullName, admin.Email, admin.Role);
        }
    }
}
