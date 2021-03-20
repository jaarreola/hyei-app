using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Cotizacion
    {
        public int CotizacionId { get; set; }
        public int OrdenTrabajoDetalleId { get; set; }
        [StringLength(500)]
        public string Comentario { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }

        public virtual OrdenTrabajoDetalle OrdenTrabajoDetalle { get; set; }
    }
}
