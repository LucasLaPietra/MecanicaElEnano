using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public interface IPresupuestoRepository : IRepositoryBase<Presupuesto>
    {
        new IList<PresupuestoDto> FindAll();
        Task<PresupuestoDto> FindByIdAsync(Guid id);
        Task<PresupuestoDto> CreateAsync(Guid vehiculoId);
        Task<PresupuestoDto> UpdateAsync(UpdatePresupuestoDto presupuestoDto);
        Task DeleteAsync(Guid id);
    }
}
