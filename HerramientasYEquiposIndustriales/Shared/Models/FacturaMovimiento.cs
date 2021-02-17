using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class FacturaMovimiento
    {
        public int FacturaMovimientoId { get; set; }
        [StringLength(20)]
        public string Factura { get; set; }
        [StringLength(1000)]
        public string Descripcion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }
    }
}
