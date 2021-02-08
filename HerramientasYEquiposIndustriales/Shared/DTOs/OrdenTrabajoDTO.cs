using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class OrdenTrabajoDTO
    {
        public int OrdenTrabajoId { get; set; }
        public int? ClienteId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
