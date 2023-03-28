using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendMecanicaElEnano.Models
{
    [Table("presupuesto")]
    public class Presupuesto
    {
        public Presupuesto()
        {
        }

        public Presupuesto(Guid idPresupuesto, DateTime fecha, DateTime validoHasta, int km, string trabajoARealizar, Vehiculo vehiculo)
        {
            PresupuestoId = idPresupuesto;
            Fecha = fecha;
            ValidoHasta = validoHasta;
            Km = km;
            TrabajoARealizar = trabajoARealizar;
            Vehiculo = vehiculo;
        }

        [Key]
        public Guid PresupuestoId { get; set; }

        [Required(ErrorMessage = "Se requiere una fecha para registrar un presupuesto")]
        public DateTime? Fecha { get; set; }

        public DateTime? ValidoHasta { get; set; }

        public int? Km { get; set; }

        public string? TrabajoARealizar { get; set; }

        public Guid? VehiculoId { get; set; }
        public virtual Vehiculo Vehiculo { get; set; } = null!;

        public virtual ICollection<Repuesto>? Repuestos { get; set; } = new Collection<Repuesto>();
    }
}
