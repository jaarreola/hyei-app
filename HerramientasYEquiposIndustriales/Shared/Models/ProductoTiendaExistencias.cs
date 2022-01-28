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
        public bool ParaRenta { get; set; }
        [DataType(DataType.Currency)]
        public float? PrecioRenta { get; set; }
        public bool ParaVenta { get; set; }
        [DataType(DataType.Currency)]
        public float? PrecioVenta { get; set; }
        public bool NoDisponibleRenta { get; set; }
        public bool Vendido { get; set; }
        public DateTime? FechaVendido { get; set; }
        public int EmpleadoVendio { get; set; }
        public int ClienteVendido { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int EmpleadoBaja { get; set; }

        public virtual ProductosTienda ProductoTienda { get; set; }
    }
}
