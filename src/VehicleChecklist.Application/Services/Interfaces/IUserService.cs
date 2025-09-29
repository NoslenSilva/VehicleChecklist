using VehicleChecklist.Application.DTOs;

namespace VehicleChecklist.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterAsync(RegisterUserDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<List<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto> SeedAdminAsync();
    }
}
