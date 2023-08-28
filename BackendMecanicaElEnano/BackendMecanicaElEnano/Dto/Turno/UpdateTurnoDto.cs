namespace BackendMecanicaElEnano.Dto
{
    public class UpdateTurnoDto
    {
        public Guid TurnoId { get; set; }

        public DateTime FechayHora { get; set; }

        public string Detalle { get; set; } = null!;
    }
}
