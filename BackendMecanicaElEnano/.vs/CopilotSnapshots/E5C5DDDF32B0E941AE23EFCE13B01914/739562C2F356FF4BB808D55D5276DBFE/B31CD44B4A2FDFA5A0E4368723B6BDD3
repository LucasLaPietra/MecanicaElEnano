using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public interface IOrdenTrabajoRepository : IRepositoryBase<OrdenTrabajo>
    {
        new IList<OrdenTrabajoDto> FindAll();
        Task<OrdenTrabajoDto> FindByIdAsync(Guid id);
        Task<OrdenTrabajoDto> CreateAsync(Guid vehiculoId);
        Task<OrdenTrabajoDto> UpdateAsync(UpdateOrdenTrabajoDto trabajoDto);
        Task DeleteAsync(Guid id);
    }
}
