using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;

namespace BackendMecanicaElEnano.Services
{
    /// <summary>
    /// Service interface for Vehiculo business logic
    /// </summary>
    public interface IVehiculoService
    {
        Task<Result<VehiculoDto>> GetByIdAsync(Guid id);
        Task<Result<IList<VehiculoDto>>> GetAllAsync();
        Task<Result<VehiculoDto>> CreateAsync(CreateVehiculoDto dto);
        Task<Result<VehiculoDto>> UpdateAsync(VehiculoDto dto);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<VehiculoDto>> GetByPatenteAsync(string patente);
    }
}
