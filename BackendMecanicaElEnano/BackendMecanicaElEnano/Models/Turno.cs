using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendMecanicaElEnano.Models
{
    [Table("turno")]
    public class Turno
    {
        [Key]
        public Guid TurnoId { get; set; }

        [Required(ErrorMessage = "Se requiere una fecha para registrar un turno")]
        public DateTime FechayHora { get; set; }

        public virtual Vehiculo Vehiculo { get; set; } = null!;
    }
}
