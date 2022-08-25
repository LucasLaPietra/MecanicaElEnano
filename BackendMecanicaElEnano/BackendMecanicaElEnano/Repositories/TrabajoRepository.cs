using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Repositories
{
    public class TrabajoRepository : RepositoryBase<Trabajo>, ITrabajoRepository
    {
        public TrabajoRepository(MecanicaContext mecanicaContext)
    : base(mecanicaContext)
        {
        }
    }
}
