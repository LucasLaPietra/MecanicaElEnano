using AutoMapper;
using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using BackendMecanicaElEnano.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BackendMecanicaElEnano.Services
{
    public class OrdenTrabajoService : IOrdenTrabajoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdenTrabajoService> _logger;

        public OrdenTrabajoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrdenTrabajoService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IList<OrdenTrabajoDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all ordenes de trabajo");
                var ordenes = await _unitOfWork.OrdenesTrabajo.FindAll().ToListAsync();
                var ordenesDto = _mapper.Map<IList<OrdenTrabajoDto>>(ordenes);

                _logger.LogInformation("Successfully retrieved {Count} ordenes de trabajo", ordenesDto.Count);
                return Result.Success(ordenesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all ordenes de trabajo");
                return Result.Failure<IList<OrdenTrabajoDto>>("Error al obtener las órdenes de trabajo");
            }
        }

        public async Task<Result<OrdenTrabajoDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving orden de trabajo with ID: {Id}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid orden de trabajo ID provided: {Id}", id);
                    return Result.Failure<OrdenTrabajoDto>("El ID de la orden de trabajo no es válido");
                }

                var orden = await _unitOfWork.OrdenesTrabajo
                    .FindByCondition(o => o.OrdenTrabajoId == id)
                    .FirstOrDefaultAsync();

                if (orden == null)
                {
                    _logger.LogWarning("Orden de trabajo with ID {Id} not found", id);
                    return Result.Failure<OrdenTrabajoDto>($"No se encontró la orden de trabajo con ID {id}");
                }

                var ordenDto = _mapper.Map<OrdenTrabajoDto>(orden);
                _logger.LogInformation("Successfully retrieved orden de trabajo with ID: {Id}", id);

                return Result.Success(ordenDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orden de trabajo with ID: {Id}", id);
                return Result.Failure<OrdenTrabajoDto>("Error al obtener la orden de trabajo");
            }
        }

        public async Task<Result<OrdenTrabajoDto>> CreateAsync(Guid vehiculoId)
        {
            try
            {
                _logger.LogInformation("Creating orden de trabajo for vehicle ID: {VehiculoId}", vehiculoId);

                if (vehiculoId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid vehicle ID provided: {VehiculoId}", vehiculoId);
                    return Result.Failure<OrdenTrabajoDto>("El ID del vehículo no es válido");
                }

                var vehiculoExists = await _unitOfWork.Vehiculos
                    .FindByCondition(v => v.VehiculoId == vehiculoId)
                    .AnyAsync();

                if (!vehiculoExists)
                {
                    _logger.LogWarning("Vehicle with ID {VehiculoId} not found", vehiculoId);
                    return Result.Failure<OrdenTrabajoDto>($"No se encontró el vehículo con ID {vehiculoId}");
                }

                var ordenTrabajo = new OrdenTrabajo
                {
                    OrdenTrabajoId = Guid.NewGuid(),
                    VehiculoId = vehiculoId,
                    Fecha = DateTime.Today,
                    Km = 0,
                    Manifiesto = string.Empty,
                    Mecanico = string.Empty
                };

                _unitOfWork.OrdenesTrabajo.Create(ordenTrabajo);
                await _unitOfWork.CommitAsync();

                var ordenDto = _mapper.Map<OrdenTrabajoDto>(ordenTrabajo);
                _logger.LogInformation("Successfully created orden de trabajo with ID: {Id}", ordenTrabajo.OrdenTrabajoId);

                return Result.Success(ordenDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating orden de trabajo for vehicle ID: {VehiculoId}", vehiculoId);
                await _unitOfWork.RollbackAsync();
                return Result.Failure<OrdenTrabajoDto>("Error al crear la orden de trabajo");
            }
        }

        public async Task<Result<OrdenTrabajoDto>> UpdateAsync(UpdateOrdenTrabajoDto dto)
        {
            try
            {
                _logger.LogInformation("Updating orden de trabajo with ID: {Id}", dto.OrdenTrabajoId);

                if (dto.OrdenTrabajoId == Guid.Empty)
                {
                    _logger.LogWarning("Invalid orden de trabajo ID provided");
                    return Result.Failure<OrdenTrabajoDto>("El ID de la orden de trabajo no es válido");
                }

                if (dto.Km < 0)
                {
                    _logger.LogWarning("Invalid kilometers value: {Km}", dto.Km);
                    return Result.Failure<OrdenTrabajoDto>("El kilometraje no puede ser negativo");
                }

                if (string.IsNullOrWhiteSpace(dto.Manifiesto))
                {
                    return Result.Failure<OrdenTrabajoDto>("El manifiesto es requerido");
                }

                var orden = await _unitOfWork.OrdenesTrabajo
                    .FindByCondition(o => o.OrdenTrabajoId == dto.OrdenTrabajoId)
                    .FirstOrDefaultAsync();

                if (orden == null)
                {
                    _logger.LogWarning("Orden de trabajo with ID {Id} not found for update", dto.OrdenTrabajoId);
                    return Result.Failure<OrdenTrabajoDto>($"No se encontró la orden de trabajo con ID {dto.OrdenTrabajoId}");
                }

                orden.Manifiesto = dto.Manifiesto;
                orden.Mecanico = dto.Mecanico;
                orden.Fecha = dto.Fecha;
                orden.Km = dto.Km;

                _unitOfWork.OrdenesTrabajo.Update(orden);
                await _unitOfWork.CommitAsync();

                var updatedDto = _mapper.Map<OrdenTrabajoDto>(orden);
                _logger.LogInformation("Successfully updated orden de trabajo with ID: {Id}", orden.OrdenTrabajoId);

                return Result.Success(updatedDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating orden de trabajo with ID: {Id}", dto.OrdenTrabajoId);
                await _unitOfWork.RollbackAsync();
                return Result.Failure<OrdenTrabajoDto>("Error al actualizar la orden de trabajo");
            }
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting orden de trabajo with ID: {Id}", id);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid orden de trabajo ID provided: {Id}", id);
                    return Result.Failure("El ID de la orden de trabajo no es válido");
                }

                var orden = await _unitOfWork.OrdenesTrabajo
                    .FindByCondition(o => o.OrdenTrabajoId == id)
                    .FirstOrDefaultAsync();

                if (orden == null)
                {
                    _logger.LogWarning("Orden de trabajo with ID {Id} not found for deletion", id);
                    return Result.Failure($"No se encontró la orden de trabajo con ID {id}");
                }

                _unitOfWork.OrdenesTrabajo.Delete(orden);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Successfully deleted orden de trabajo with ID: {Id}", id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting orden de trabajo with ID: {Id}", id);
                await _unitOfWork.RollbackAsync();
                return Result.Failure("Error al eliminar la orden de trabajo");
            }
        }
    }
}
