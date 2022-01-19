using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class ProductosTiendaDTO
    {
        public int ProductosTiendaId { get; set; }
        public string Sku { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoCompra { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoVenta { get; set; }
        public string Caracteristicas { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int EmpleadoCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int EmpleadoModificacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int EmpleadoBaja { get; set; }
        public DateTime? FechaActivo { get; set; }
        public int EmpleadoActivo { get; set; }

    }
}
