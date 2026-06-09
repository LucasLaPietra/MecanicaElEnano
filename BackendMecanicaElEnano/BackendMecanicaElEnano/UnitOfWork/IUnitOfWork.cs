using BackendMecanicaElEnano.Repositories;

namespace BackendMecanicaElEnano.UnitOfWork
{
    /// <summary>
    /// Unit of Work pattern - manages transactions across multiple repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IPresupuestoRepository Presupuestos { get; }
        IVehiculoRepository Vehiculos { get; }
        ITrabajoRepository Trabajos { get; }
        ITurnoRepository Turnos { get; }
        IOrdenTrabajoRepository OrdenesTrabajo { get; }

        Task<int> CommitAsync();
        Task RollbackAsync();
    }
}
