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
        public Turno(DateTime fechayHora, string detalle)
        {
            FechayHora = fechayHora;
            Detalle = detalle;
        }

        public Turno()
        {
            
        }


        [Key]
        public Guid TurnoId { get; set; }

        [Required(ErrorMessage = "Se requiere una fecha para registrar un turno")]
        public DateTime FechayHora { get; set; }

        public string Detalle { get; set; } = null!;

        public virtual Vehiculo Vehiculo { get; set; } = null!;
    }
}
