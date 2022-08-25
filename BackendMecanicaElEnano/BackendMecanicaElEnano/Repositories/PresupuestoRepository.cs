using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public class PresupuestoRepository : RepositoryBase<Presupuesto>, IPresupuestoRepository
    {
        public PresupuestoRepository(MecanicaContext mecanicaContext)
    : base(mecanicaContext)
        {
        }
    }
}
