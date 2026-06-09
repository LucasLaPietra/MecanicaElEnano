using BackendMecanicaElEnano.Repositories;

namespace BackendMecanicaElEnano.UnitOfWork
{
    /// <summary>
    /// Implements Unit of Work pattern for coordinating the work of multiple repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MecanicaContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public IPresupuestoRepository Presupuestos { get; }
        public IVehiculoRepository Vehiculos { get; }
        public ITrabajoRepository Trabajos { get; }
        public ITurnoRepository Turnos { get; }
        public IOrdenTrabajoRepository OrdenesTrabajo { get; }

        public UnitOfWork(
            MecanicaContext context,
            IPresupuestoRepository presupuestos,
            IVehiculoRepository vehiculos,
            ITrabajoRepository trabajos,
            ITurnoRepository turnos,
            IOrdenTrabajoRepository ordenesTrabajo,
            ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
            Presupuestos = presupuestos;
            Vehiculos = vehiculos;
            Trabajos = trabajos;
            Turnos = turnos;
            OrdenesTrabajo = ordenesTrabajo;
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                var changes = await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully saved {Count} changes to the database", changes);
                return changes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to the database");
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await Task.Run(() =>
            {
                foreach (var entry in _context.ChangeTracker.Entries())
                {
                    entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                }
                _logger.LogInformation("Transaction rolled back");
            });
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
