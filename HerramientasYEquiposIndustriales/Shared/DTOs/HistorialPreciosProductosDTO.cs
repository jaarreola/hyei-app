using System;
using System.ComponentModel.DataAnnotations;
using HerramientasYEquiposIndustriales.Shared.Models;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class HistorialPreciosProductosDTO
    {
        public int HistorialPreciosProductosId { get; set; }
        public int ProductoId { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoCompra { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoVenta { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        
        public virtual Producto Producto { get; set; }
    }
}
