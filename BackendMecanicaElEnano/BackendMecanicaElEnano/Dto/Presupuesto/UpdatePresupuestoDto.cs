using System.Collections.ObjectModel;

namespace BackendMecanicaElEnano.Dto
{
    public class UpdatePresupuestoDto
    {
        public Guid PresupuestoId { get; set; }
        public DateTime Fecha { get; set; }

        public DateTime ValidoHasta { get; set; }

        public int Km { get; set; }

        public string TrabajoARealizar { get; set; } = null!;

        public ICollection<RepuestoDto> Repuestos { get; set; } = new Collection<RepuestoDto>();
    }
}
