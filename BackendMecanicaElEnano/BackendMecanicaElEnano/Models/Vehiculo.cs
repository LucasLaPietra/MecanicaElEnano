using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendMecanicaElEnano.Models
{
    [Table("vehiculo")]
    public class Vehiculo
    {
        public Vehiculo()
        {
        }

        public Vehiculo(Guid idVehiculo, string patente, string modelo, string numeroChasis, string cliente, string telefono, string direccion, string mail, string cuit)
        {
            VehiculoId = idVehiculo;
            Patente = patente;
            Modelo = modelo;
            NumeroChasis = numeroChasis;
            Cliente = cliente;
            Telefono = telefono;
            Direccion = direccion;
            Mail = mail;
            Cuit = cuit;
        }

        [Key]
        public Guid VehiculoId { get; set; }

        [Required(ErrorMessage = "Se requiere una patente para registrar un vehiculo")]
        [StringLength(7, ErrorMessage = "La patente debe tener 6 o 7 caracteres")]
        public string Patente { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un modelo para registrar un vehiculo")]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un numero de chasis para registrar un vehiculo")]
        public string? NumeroChasis { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un nombre de cliente para registrar un vehiculo")]
        public string Cliente { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un telefono de cliente para registrar un vehiculo")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere una direccion del cliente para registrar un vehiculo")]
        [StringLength(60, ErrorMessage = "La direccion es demasiado larga")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un mail del cliente para registrar un vehiculo")]
        [StringLength(60, ErrorMessage = "El mail es demasiado largo")]
        public string? Mail { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un cuit de cliente para registrar un vehiculo")]
        [StringLength(11, ErrorMessage = "El Cuit debe tener 11 digitos")]
        public string? Cuit { get; set; } = null!;

        public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new Collection<Presupuesto>();

        public virtual ICollection<Trabajo> Trabajos { get; set; } = new Collection<Trabajo>();

        public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; } = new Collection<OrdenTrabajo>();

    }
}
