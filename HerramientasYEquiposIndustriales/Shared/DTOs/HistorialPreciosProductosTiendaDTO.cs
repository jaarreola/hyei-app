using System;
using System.ComponentModel.DataAnnotations;
using HerramientasYEquiposIndustriales.Shared.Models;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class HistorialPreciosProductosTiendaDTO
    {
        public int HistorialPreciosProductosTiendaId { get; set; }
        public int ProductosTiendaId { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoCompra { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoVenta { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        
        public virtual ProductosTienda Producto { get; set; }
    }
}
