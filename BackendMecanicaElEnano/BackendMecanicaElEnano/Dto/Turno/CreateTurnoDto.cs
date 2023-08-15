

namespace BackendMecanicaElEnano.Dto
{ 
    public class CreateTurnoDto
    {
        public DateTime FechayHora { get; set; } 

        public virtual Guid VehiculoId { get; set; } 
    }
}
