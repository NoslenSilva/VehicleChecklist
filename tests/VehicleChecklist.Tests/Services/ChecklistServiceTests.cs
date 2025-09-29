using Moq;
using VehicleChecklist.Application.Services;
using VehicleChecklist.Application.Services.Interfaces;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Tests.Services
{
    public class ChecklistServiceTests
    {
        private readonly Mock<IChecklistRepository> _checklistRepoMock;
        private readonly Mock<IVehicleRepository> _vehicleRepoMock;
        private readonly IChecklistService _service;

        public ChecklistServiceTests()
        {
            _checklistRepoMock = new Mock<IChecklistRepository>();
            _vehicleRepoMock = new Mock<IVehicleRepository>();
            _service = new ChecklistService(_checklistRepoMock.Object, _vehicleRepoMock.Object);
        }

        [Fact]
        public async Task StartChecklistAsync_Should_Create_New_Checklist()
        {
            var vehicleId = Guid.NewGuid();
            var executor = new User { Id = Guid.NewGuid(), Role = UserRole.Executor };

            _checklistRepoMock.Setup(r => r.GetInProgressByVehicleAsync(vehicleId))
                .ReturnsAsync((Checklist?)null);

            var checklist = await _service.StartChecklistAsync(vehicleId, executor);

            Assert.Equal(vehicleId, checklist.VehicleId);
            Assert.Equal(executor.Id, checklist.StartedById);
            Assert.Equal(ChecklistStatus.InProgress, checklist.Status);

            _checklistRepoMock.Verify(r => r.AddAsync(It.IsAny<Checklist>()), Times.Once);
            _checklistRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task StartChecklistAsync_Should_Throw_When_Checklist_InProgress_Exists()
        {
            var vehicleId = Guid.NewGuid();
            var executor = new User { Id = Guid.NewGuid(), Role = UserRole.Executor };

            _checklistRepoMock.Setup(r => r.GetInProgressByVehicleAsync(vehicleId))
                .ReturnsAsync(new Checklist { Id = Guid.NewGuid(), VehicleId = vehicleId, Status = ChecklistStatus.InProgress });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.StartChecklistAsync(vehicleId, executor));
        }
    }
}
