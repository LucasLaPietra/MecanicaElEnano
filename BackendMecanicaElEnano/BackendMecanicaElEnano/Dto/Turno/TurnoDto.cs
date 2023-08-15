

namespace BackendMecanicaElEnano.Dto
{
    public class TurnoDto
    {
        public Guid TurnoId { get; set; }

        public DateTime FechayHora { get; set; }

        public virtual VehiculoDto? Vehiculo { get; set; }
    }
}
