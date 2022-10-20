using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public interface IVehiculoRepository:IRepositoryBase<Vehiculo>
    {
        new IList<VehiculoDto> FindAll();
        Task<VehiculoDto> FindByIdAsync(Guid id);
        Task<VehiculoDto> CreateAsync(CreateVehiculoDto createVehiculoDto);
        Task<VehiculoDto> UpdateAsync(VehiculoDto vehiculoDto);
        Task DeleteAsync(Guid id);
    }
}
