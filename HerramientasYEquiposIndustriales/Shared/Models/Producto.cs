using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        [Required]
        [StringLength(50)]
        public string NoParte { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public int MarcaId { get; set; }
        [StringLength(50)]
        public string Modelo { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoCompra { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoVenta { get; set; }
        public float? CantidadMinimaInventario { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int? EmpleadoBaja { get; set; }
        public DateTime? FechaActivo { get; set; }
        public int? EmpleadoActivo { get; set; }
        [StringLength(500)]
        public String Ubicacion { get; set; }


        public virtual Marca Marca { get; set; }
    }
}
