using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class CotizacionDetalleDTO
    {
        public int CotizacionDetalleId { get; set; }
        public int CotizacionId { get; set; }
        public int ProductoId { get; set; }
        public float? Cantidad { get; set; }
        public float? CostoUnitario { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }

        public virtual Producto Producto { get; set; }
        public virtual Cotizacion Cotizacion { get; set; }
    }
}
