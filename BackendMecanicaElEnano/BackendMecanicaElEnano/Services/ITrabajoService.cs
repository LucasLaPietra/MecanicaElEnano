using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;

namespace BackendMecanicaElEnano.Services
{
    public interface ITrabajoService
    {
        Task<Result<TrabajoDto>> GetByIdAsync(Guid id);
        Task<Result<IList<TrabajoDto>>> GetAllAsync();
        Task<Result<TrabajoDto>> CreateAsync(Guid vehiculoId);
        Task<Result<TrabajoDto>> UpdateAsync(UpdateTrabajoDto dto);
        Task<Result> DeleteAsync(Guid id);
    }
}
