namespace BackendMecanicaElEnano.Dto
{
    public class CreateVehiculoDto
    {
        public string Patente { get; set; } = null!;

        public string Modelo { get; set; } = null!;

        public string NumeroChasis { get; set; } = string.Empty;

        public string Cliente { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Mail { get; set; } = string.Empty;

        public string Cuit { get; set; } = string.Empty;
    }
}
