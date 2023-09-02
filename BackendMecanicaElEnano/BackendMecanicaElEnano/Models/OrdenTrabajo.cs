using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendMecanicaElEnano.Models
{
    [Table("ordenTrabajo")]
    public class OrdenTrabajo
    {
        [Key]
        public Guid OrdenTrabajoId { get; set; }

        [Required(ErrorMessage = "Se requiere una fecha para registrar un trabajo")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Se requiere un kilometraje para registrar un presupuesto")]
        public int Km { get; set; }

        [Required(ErrorMessage = "Se requiere un trabajo realizado para registrar un trabajo")]
        public string Manifiesto { get; set; } = null!;

        public string Mecanico { get; set; } = null!;

        public Guid? VehiculoId { get; set; }

        public virtual Vehiculo Vehiculo { get; set; } = null!;
    }
}
