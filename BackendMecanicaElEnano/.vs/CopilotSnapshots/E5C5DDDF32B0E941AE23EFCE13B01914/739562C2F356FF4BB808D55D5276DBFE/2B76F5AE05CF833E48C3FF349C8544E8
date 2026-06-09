using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public interface ITurnoRepository : IRepositoryBase<Turno>
    {
        new IList<TurnoDto> FindAll();
        Task<TurnoDto> FindByIdAsync(Guid id);
        Task<TurnoDto> CreateAsync(CreateTurnoDto createTurnoDto);
        Task<TurnoDto> UpdateAsync(TurnoDto TurnoDto);
        Task DeleteAsync(Guid id);
    }
}
