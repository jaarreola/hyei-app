using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class HistorialPreciosProductos
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
