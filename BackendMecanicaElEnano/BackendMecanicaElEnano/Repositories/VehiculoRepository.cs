using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public class VehiculoRepository:RepositoryBase<Vehiculo>, IVehiculoRepository
    {
        public VehiculoRepository(MecanicaContext mecanicaContext)
    : base(mecanicaContext)
        {
        }
    }
}
