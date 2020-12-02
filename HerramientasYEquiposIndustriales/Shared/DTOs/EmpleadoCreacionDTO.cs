using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class EmpleadoCreacionDTO
    {
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
    }
}
