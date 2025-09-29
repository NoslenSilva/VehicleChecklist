using Microsoft.Extensions.Configuration;
using Moq;
using VehicleChecklist.Application.DTOs;
using VehicleChecklist.Application.Services;
using VehicleChecklist.Application.Services.Interfaces;
using VehicleChecklist.Domain.Entities;
using VehicleChecklist.Domain.Enums;
using VehicleChecklist.Infrastructure.Repositories.Interfaces;

namespace VehicleChecklist.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly IUserService _service;
        private readonly IConfiguration _config;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();

            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Secret", "test-secret-key-1234567890"}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _service = new UserService(_userRepoMock.Object, _config);
        }

        [Fact]
        public async Task RegisterAsync_Should_Create_User()
        {
            var dto = new RegisterUserDto("Executor Teste", "exec@test.com", "123456", UserRole.Executor);
            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);

            var result = await _service.RegisterAsync(dto);

            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(UserRole.Executor, result.Role);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Should_Throw_When_Email_Already_Exists()
        {
            var dto = new RegisterUserDto("Executor Teste", "exec@test.com", "123456", UserRole.Executor);
            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = dto.Email });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RegisterAsync(dto));
        }
    }
}
