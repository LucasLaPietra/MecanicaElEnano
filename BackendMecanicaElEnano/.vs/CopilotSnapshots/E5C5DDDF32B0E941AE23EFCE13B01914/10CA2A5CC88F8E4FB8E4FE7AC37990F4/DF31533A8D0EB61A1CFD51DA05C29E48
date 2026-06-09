using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public interface ITrabajoRepository : IRepositoryBase<Trabajo>
    {
        new IList<TrabajoDto> FindAll();
        Task<TrabajoDto> FindByIdAsync(Guid id);
        Task<TrabajoDto> CreateAsync(Guid vehiculoId);
        Task<TrabajoDto> UpdateAsync(UpdateTrabajoDto trabajoDto);
        Task DeleteAsync(Guid id);
    }
}
