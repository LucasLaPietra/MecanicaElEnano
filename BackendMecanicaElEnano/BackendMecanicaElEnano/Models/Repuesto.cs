﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendMecanicaElEnano.Models
{
    [Table("repuesto")]
    public class Repuesto
    {
        [Key]
        public Guid RepuestoId { get; set; }

        [Required(ErrorMessage = "Se requiere una cantidad para registrar un repuesto")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Se requiere una descripcion para registrar un repuesto")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Se requiere un precio para registrar un repuesto")]
        public float precio { get; set; }

        [Required(ErrorMessage = "Se requiere un tipo de trabajo para registrar un repuesto")]
        public TipoTrabajo Tipo { get; set; }

        public virtual Presupuesto Presupuesto { get; set; } = null!;
    }
}
