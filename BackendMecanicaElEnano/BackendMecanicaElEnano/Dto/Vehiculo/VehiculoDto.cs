using System.Collections.ObjectModel;

namespace BackendMecanicaElEnano.Dto
{
    public class VehiculoDto
    {
            public Guid VehiculoId { get; set; }

            public string Patente { get; set; } = null!;

            public string Modelo { get; set; } = null!;

            public string NumeroChasis { get; set; } = null!;

            public string Cliente { get; set; } = null!;

            public string Telefono { get; set; } = null!;

            public string Direccion { get; set; } = null!;

            public string Mail { get; set; } = null!;

            public string Cuit { get; set; } = null!;

            public ICollection<PresupuestoDto> Presupuestos { get; set; }= new Collection<PresupuestoDto>();

            public virtual ICollection<TrabajoDto> Trabajos { get; set; } = new Collection<TrabajoDto>();

            public virtual ICollection<OrdenTrabajoDto> OrdenTrabajos { get; set; } = new Collection<OrdenTrabajoDto>();

    }
}
