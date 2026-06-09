using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;

namespace BackendMecanicaElEnano.Services
{
    public interface IOrdenTrabajoService
    {
        Task<Result<OrdenTrabajoDto>> GetByIdAsync(Guid id);
        Task<Result<IList<OrdenTrabajoDto>>> GetAllAsync();
        Task<Result<OrdenTrabajoDto>> CreateAsync(Guid vehiculoId);
        Task<Result<OrdenTrabajoDto>> UpdateAsync(UpdateOrdenTrabajoDto dto);
        Task<Result> DeleteAsync(Guid id);
    }
}
