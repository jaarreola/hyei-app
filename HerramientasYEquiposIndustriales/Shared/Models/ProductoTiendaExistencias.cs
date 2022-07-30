using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class ProductoTiendaExistencias
    {
        public int ProductoTiendaExistenciasId { get; set; }
        public int ProductoTiendaId { get; set; }
        public int FolioProductoTienda { get; set; }
        public bool Usado { get; set; }
        public string Comentarios { get; set; }
        public bool ParaVenta { get; set; }
        [DataType(DataType.Currency)]
        public float? PrecioVenta { get; set; }

        public bool ParaRenta { get; set; }
        public int? TotalHorasRentado { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoDia { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoSemana { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoQuincena { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoMes { get; set; }

        public bool Rentado { get; set; }
        public bool Vendido { get; set; }
        public DateTime? FechaVendido { get; set; }
        public int EmpleadoVendio { get; set; }
        public int ClienteVendido { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int EmpleadoBaja { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int EmpleadoRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }

        public virtual ProductosTienda ProductoTienda { get; set; }
    }
}
