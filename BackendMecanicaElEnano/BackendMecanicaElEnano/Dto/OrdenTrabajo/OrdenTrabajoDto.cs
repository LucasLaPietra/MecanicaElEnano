namespace BackendMecanicaElEnano.Dto
{
    public class OrdenTrabajoDto
    {
        public Guid OrdenTrabajoId { get; set; }

        public DateTime Fecha { get; set; }

        public int Km { get; set; }

        public string Manifiesto { get; set; } = null!;

        public string Mecanico { get; set; } = null!;
    }
}
