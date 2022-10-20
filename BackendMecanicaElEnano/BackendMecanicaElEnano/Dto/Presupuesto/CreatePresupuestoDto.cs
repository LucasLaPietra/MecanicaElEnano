using System.Collections.ObjectModel;

namespace BackendMecanicaElEnano.Dto
{
    public class CreatePresupuestoDto
    {
        public DateTime Fecha { get; set; }

        public DateTime ValidoHasta { get; set; }

        public int Km { get; set; }

        public string TrabajoARealizar { get; set; } = null!;

        public Guid VehiculoId { get; set; }

        public ICollection<CreateRepuestoDto> Repuestos { get; set; } = new Collection<CreateRepuestoDto>();
    }
}
