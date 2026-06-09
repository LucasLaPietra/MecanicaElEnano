using AutoMapper;
using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using BackendMecanicaElEnano.Services;
using BackendMecanicaElEnano.UnitOfWork;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace BackendMecanicaElEnano.Tests.Services
{
    /// <summary>
    /// Unit tests for PresupuestoService
    /// Tests all business logic scenarios using mocks
    /// </summary>
    public class PresupuestoServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PresupuestoService>> _mockLogger;
        private readonly PresupuestoService _service;
        private readonly Mock<IPresupuestoRepository> _mockPresupuestoRepo;
        private readonly Mock<IVehiculoRepository> _mockVehiculoRepo;

        public PresupuestoServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PresupuestoService>>();
            _mockPresupuestoRepo = new Mock<IPresupuestoRepository>();
            _mockVehiculoRepo = new Mock<IVehiculoRepository>();

            // Setup UnitOfWork to return our mocked repositories
            _mockUnitOfWork.Setup(u => u.Presupuestos).Returns(_mockPresupuestoRepo.Object);
            _mockUnitOfWork.Setup(u => u.Vehiculos).Returns(_mockVehiculoRepo.Object);

            _service = new PresupuestoService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var presupuestoId = Guid.NewGuid();
            var presupuesto = new Presupuesto { PresupuestoId = presupuestoId };
            var presupuestoDto = new PresupuestoDto { PresupuestoId = presupuestoId };

            var mockQueryable = new List<Presupuesto> { presupuesto }.AsQueryable().BuildMock();

            _mockPresupuestoRepo
                .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Presupuesto, bool>>>()))
                .Returns(mockQueryable);

            _mockMapper.Setup(m => m.Map<PresupuestoDto>(It.IsAny<Presupuesto>()))
                .Returns(presupuestoDto);

            // Act
            var result = await _service.GetByIdAsync(presupuestoId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.PresupuestoId.Should().Be(presupuestoId);
        }

        [Fact]
        public async Task GetByIdAsync_WithEmptyGuid_ReturnsFailure()
        {
            // Act
            var result = await _service.GetByIdAsync(Guid.Empty);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("no es válido");
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ReturnsFailure()
        {
            // Arrange
            var presupuestoId = Guid.NewGuid();
            var mockQueryable = new List<Presupuesto>().AsQueryable().BuildMock();

            _mockPresupuestoRepo
                .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Presupuesto, bool>>>()))
                .Returns(mockQueryable);

            // Act
            var result = await _service.GetByIdAsync(presupuestoId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("no se encontró");
        }

        [Fact]
        public async Task CreateAsync_WithValidVehiculoId_ReturnsSuccess()
        {
            // Arrange
            var vehiculoId = Guid.NewGuid();
            var mockVehiculoQueryable = new List<Vehiculo>
            {
                new Vehiculo { VehiculoId = vehiculoId }
            }.AsQueryable().BuildMock();

            _mockVehiculoRepo
                .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Vehiculo, bool>>>()))
                .Returns(mockVehiculoQueryable);

            _mockPresupuestoRepo.Setup(r => r.Create(It.IsAny<Presupuesto>()));
            _mockUnitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            _mockMapper.Setup(m => m.Map<PresupuestoDto>(It.IsAny<Presupuesto>()))
                .Returns(new PresupuestoDto { PresupuestoId = Guid.NewGuid() });

            // Act
            var result = await _service.CreateAsync(vehiculoId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            _mockPresupuestoRepo.Verify(r => r.Create(It.IsAny<Presupuesto>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithEmptyGuid_ReturnsFailure()
        {
            // Act
            var result = await _service.CreateAsync(Guid.Empty);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("no es válido");
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenVehiculoNotFound_ReturnsFailure()
        {
            // Arrange
            var vehiculoId = Guid.NewGuid();
            var mockVehiculoQueryable = new List<Vehiculo>().AsQueryable().BuildMock();

            _mockVehiculoRepo
                .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Vehiculo, bool>>>()))
                .Returns(mockVehiculoQueryable);

            // Act
            var result = await _service.CreateAsync(vehiculoId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("no se encontró el vehículo");
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var presupuestoId = Guid.NewGuid();
            var updateDto = new UpdatePresupuestoDto
            {
                PresupuestoId = presupuestoId,
                Fecha = DateTime.Today,
                ValidoHasta = DateTime.Today.AddDays(30),
                Km = 5000,
                TrabajoARealizar = "Test",
                Repuestos = new List<RepuestoDto>()
            };

            var presupuesto = new Presupuesto
            {
                PresupuestoId = presupuestoId,
                Repuestos = new List<Repuesto>()
            };

            var mockQueryable = new List<Presupuesto> { presupuesto }.AsQueryable().BuildMock();

            _mockPresupuestoRepo
                .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Presupuesto, bool>>>()))
                .Returns(mockQueryable);

            _mockPresupuestoRepo.Setup(r => r.Update(It.IsAny<Presupuesto>()));
            _mockUnitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            _mockMapper.Setup(m => m.Map<List<Repuesto>>(It.IsAny<List<RepuestoDto>>()))
                .Returns(new List<Repuesto>());

            _mockMapper.Setup(m => m.Map<PresupuestoDto>(It.IsAny<Presupuesto>()))
                .Returns(new PresupuestoDto { PresupuestoId = presupuestoId });

            // Act
            var result = await _service.UpdateAsync(updateDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockPresupuestoRepo.Verify(r => r.Update(It.IsAny<Presupuesto>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidDateRange_ReturnsFailure()
        {
            // Arrange
            var updateDto = new UpdatePresupuestoDto
            {
                PresupuestoId = Guid.NewGuid(),
                Fecha = DateTime.Today.AddDays(30),
                ValidoHasta = DateTime.Today, // Before Fecha!
                Km = 5000
            };

            // Act
            var result = await _service.UpdateAsync(updateDto);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("fecha de inicio no puede ser posterior");
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WithNegativeKm_ReturnsFailure()
        {
            // Arrange
            var updateDto = new UpdatePresupuestoDto
            {
                PresupuestoId = Guid.NewGuid(),
                Fecha = DateTime.Today,
                ValidoHasta = DateTime.Today.AddDays(30),
                Km = -100 // Negative!
            };

            // Act
            var result = await _service.UpdateAsync(updateDto);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("kilometraje no puede ser negativo");
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var presupuestoId = Guid.NewGuid();
            var presupuesto = new Presupuesto { PresupuestoId = presupuestoId };
            var mockQueryable = new List<Presupuesto> { presupuesto }.AsQueryable().BuildMock();

            _mockPresupuestoRepo
                .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Presupuesto, bool>>>()))
                .Returns(mockQueryable);

            _mockPresupuestoRepo.Setup(r => r.Delete(It.IsAny<Presupuesto>()));
            _mockUnitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(presupuestoId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockPresupuestoRepo.Verify(r => r.Delete(It.IsAny<Presupuesto>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenNotFound_ReturnsFailure()
        {
            // Arrange
            var presupuestoId = Guid.NewGuid();
            var mockQueryable = new List<Presupuesto>().AsQueryable().BuildMock();

            _mockPresupuestoRepo
                .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Presupuesto, bool>>>()))
                .Returns(mockQueryable);

            // Act
            var result = await _service.DeleteAsync(presupuestoId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("no se encontró");
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsSuccessWithList()
        {
            // Arrange
            var presupuestos = new List<Presupuesto>
            {
                new Presupuesto { PresupuestoId = Guid.NewGuid() },
                new Presupuesto { PresupuestoId = Guid.NewGuid() }
            };

            var presupuestosDto = presupuestos.Select(p => new PresupuestoDto { PresupuestoId = p.PresupuestoId }).ToList();

            var mockQueryable = presupuestos.AsQueryable().BuildMock();

            _mockPresupuestoRepo.Setup(r => r.FindAll()).Returns(mockQueryable);
            _mockMapper.Setup(m => m.Map<IList<PresupuestoDto>>(It.IsAny<List<Presupuesto>>()))
                .Returns(presupuestosDto);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
        }
    }

    // Helper extension for mocking IQueryable
    public static class MockExtensions
    {
        public static IQueryable<T> BuildMock<T>(this IQueryable<T> data) where T : class
        {
            return data;
        }
    }
}
