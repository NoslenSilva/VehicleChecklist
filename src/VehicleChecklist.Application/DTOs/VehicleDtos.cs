namespace VehicleChecklist.Application.DTOs
{
    public record CreateVehicleDto(string Plate, string Model, string? Notes);
    public record UpdateVehicleDto(string Model, string? Notes);
    public record VehicleResponseDto(Guid Id, string Plate, string Model, string? Notes);
}
