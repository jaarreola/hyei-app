using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class ProductoDTO
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
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }


        public virtual Marca Marca { get; set; }
    }
}