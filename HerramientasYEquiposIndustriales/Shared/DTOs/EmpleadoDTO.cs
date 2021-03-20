using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class EmpleadoDTO
    {
        public int EmpleadoId { get; set; }
        //[Required]
        [StringLength(10)]
        public string NumeroEmpleado { get; set; }
        [Required]
        [StringLength(80)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(150)]
        public string Direccion { get; set; }
        [StringLength(10)]
        public string Telefono { get; set; }
        [StringLength(15)]
        public string Nss { get; set; }
        [StringLength(20)]
        public string Curp { get; set; }
        public int PuestoId { get; set; }
        public bool Activo { get; set; }
        public bool PuedeEditar { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        [StringLength(500)]
        public string MotivoBaja { get; set; }

        public virtual Puesto Puesto { get; set; }
    }
}
