using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        [Required]
        [StringLength(10)]
        public string NumeroEmpleado { get; set; }
        [StringLength(80)]
        public string Nombre { get; set; }
        [StringLength(150)]
        public string Direccion { get; set; }
        [StringLength(10)]
        public string Telefono { get; set; }
        public int PuestoId { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }

        public virtual Puesto Puesto { get; set; }
    }
}
