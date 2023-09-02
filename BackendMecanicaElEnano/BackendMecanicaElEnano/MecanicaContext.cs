using BackendMecanicaElEnano.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendMecanicaElEnano
{
    public class MecanicaContext : DbContext
    {
        public MecanicaContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Vehiculo>? Vehiculos { get; set; }
        public DbSet<Presupuesto>? Presupuestos { get; set; }
        public DbSet<Repuesto>? Repuestos { get; set; }
        public DbSet<Trabajo>? Trabajos { get; set; }
        public DbSet<RepuestoTrabajo>? RepuestoTrabajos { get; set; }
        public DbSet<Turno>? Turnos { get; set; }
        public DbSet<OrdenTrabajo>? OrdenTrabajos { get; set; }
    }
}
