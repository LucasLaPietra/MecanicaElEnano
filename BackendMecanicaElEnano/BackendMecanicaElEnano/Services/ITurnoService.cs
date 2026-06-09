using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;

namespace BackendMecanicaElEnano.Services
{
    public interface ITurnoService
    {
        Task<Result<TurnoDto>> GetByIdAsync(Guid id);
        Task<Result<IList<TurnoDto>>> GetAllAsync();
        Task<Result<TurnoDto>> CreateAsync(CreateTurnoDto dto);
        Task<Result<TurnoDto>> UpdateAsync(TurnoDto dto);
        Task<Result> DeleteAsync(Guid id);
    }
}
