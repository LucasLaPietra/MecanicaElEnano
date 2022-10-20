using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Dto
{
    public class RepuestoDto
    {
        public Guid? RepuestoId { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; } = null!;
        public float precio { get; set; }
        public TipoTrabajo Tipo { get; set; }
    }
}
