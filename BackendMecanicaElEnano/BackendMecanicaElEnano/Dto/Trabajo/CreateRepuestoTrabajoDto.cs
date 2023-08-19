using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Dto
{
    public class CreateRepuestoTrabajoDto
    {
        public int Cantidad { get; set; }
        public string Descripcion { get; set; } = null!;
        public float precio { get; set; }
        public TipoTrabajo Tipo { get; set; }
    }
}
