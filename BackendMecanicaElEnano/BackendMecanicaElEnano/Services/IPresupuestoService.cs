using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;

namespace BackendMecanicaElEnano.Services
{
    /// <summary>
    /// Service interface for Presupuesto business logic
    /// </summary>
    public interface IPresupuestoService
    {
        Task<Result<PresupuestoDto>> GetByIdAsync(Guid id);
        Task<Result<IList<PresupuestoDto>>> GetAllAsync();
        Task<Result<PresupuestoDto>> CreateAsync(Guid vehiculoId);
        Task<Result<PresupuestoDto>> UpdateAsync(UpdatePresupuestoDto presupuestoDto);
        Task<Result> DeleteAsync(Guid id);
    }
}
