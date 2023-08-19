using System.Collections.ObjectModel;

namespace BackendMecanicaElEnano.Dto
{
    public class CreateTrabajoDto
    {
        public DateTime Fecha { get; set; }

        public int Km { get; set; }

        public string TrabajosRealizados { get; set; } = null!;

        public string TrabajosPendientes { get; set; } = null!;

        public virtual ICollection<RepuestoTrabajoDto> Repuestos { get; set; } = new Collection<RepuestoTrabajoDto>();
    }
}
