using VehicleChecklist.Domain.Enums;

namespace VehicleChecklist.Application.DTOs
{
    public record RegisterUserDto(string FullName, string Email, string Password, UserRole Role);
    public record LoginDto(string Email, string Password);
    public record UserResponseDto(Guid Id, string FullName, string Email, UserRole Role);
}
