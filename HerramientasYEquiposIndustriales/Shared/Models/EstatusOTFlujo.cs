using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class EstatusOTFlujo
    {
        public int EstatusOTFlujoId { get; set; }
        public int OrdenTrabajoDetalleId { get; set; }
        public int EstatusOTId { get; set; }
        [StringLength(100)]
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public bool? Terminado { get; set; }
        [StringLength(1000)] 
        public string Ubicacion { get; set; }
        [StringLength(5000)]
        public string Comentario { get; set; }

        public virtual EstatusOT EstatusOT { get; set; }
        public virtual OrdenTrabajoDetalle OrdenTrabajoDetalle { get; set; }
    }
}
