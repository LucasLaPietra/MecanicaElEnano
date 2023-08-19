using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendMecanicaElEnano.Models
{
    [Table("trabajo")]
    public class Trabajo
    {
        [Key]
        public Guid TrabajoId { get; set; }

        [Required(ErrorMessage = "Se requiere una fecha para registrar un trabajo")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Se requiere un kilometraje para registrar un presupuesto")]
        public int Km { get; set; }

        [Required(ErrorMessage = "Se requiere un trabajo realizado para registrar un trabajo")]
        public string TrabajosRealizados { get; set; } = null!;

        public string TrabajosPendientes { get; set; } = null!;

        public Guid? VehiculoId { get; set; }

        public virtual Vehiculo Vehiculo { get; set; } = null!;

        public virtual ICollection<RepuestoTrabajo> Repuestos { get; set; } = new Collection<RepuestoTrabajo>();
    }
}
