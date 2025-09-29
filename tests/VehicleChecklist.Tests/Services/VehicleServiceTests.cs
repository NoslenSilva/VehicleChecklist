using Moq;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Application.Services;
using VehicleChecklist.Application.Services.Interfaces;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepoMock;
        private readonly IVehicleService _service;

        public VehicleServiceTests()
        {
            _vehicleRepoMock = new Mock<IVehicleRepository>();
            _service = new VehicleService(_vehicleRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_New_Vehicle()
        {
            var dto = new CreateVehicleDto("ABC1234", "Caminhão", "Nota teste");

            _vehicleRepoMock.Setup(r => r.GetByPlateAsync(dto.Plate)).ReturnsAsync((Vehicle?)null);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(dto.Plate.ToUpper(), result.Plate);
            Assert.Equal(dto.Model, result.Model);
            Assert.Equal(dto.Notes, result.Notes);

            _vehicleRepoMock.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
            _vehicleRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Plate_Already_Exists()
        {
            var dto = new CreateVehicleDto("ABC1234", "Caminhão", "Nota teste");
            _vehicleRepoMock.Setup(r => r.GetByPlateAsync(dto.Plate))
                .ReturnsAsync(new Vehicle { Id = Guid.NewGuid(), Plate = "ABC1234" });
                        
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        }
    }
}
