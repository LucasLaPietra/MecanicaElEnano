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
            IdPresupuesto = idPresupuesto;
            Fecha = fecha;
            ValidoHasta = validoHasta;
            Km = km;
            TrabajoARealizar = trabajoARealizar;
            Vehiculo = vehiculo;
        }

        [Key]
        public Guid IdPresupuesto { get; set; }

        [Required(ErrorMessage = "Se requiere una fecha para registrar un presupuesto")]
        public DateTime Fecha { get; set; }

        public DateTime ValidoHasta { get; set; }

        [Required(ErrorMessage = "Se requiere un kilometraje para registrar un presupuesto")]
        public int Km { get; set; }

        [Required(ErrorMessage = "Se requiere un trabajo a realizar para registrar un presupuesto")]
        public string TrabajoARealizar { get; set; } = null!;

        public Vehiculo Vehiculo { get; set; } = null!;

        public ICollection<Repuesto> Repuestos { get; set; } = new Collection<Repuesto>();
    }
}
