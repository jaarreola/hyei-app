using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Movimiento
    {
        public int MovimientoId { get; set; }
        public int ProductoId { get; set; }
        public bool EsEntrada { get; set; }
        public bool EsSalida { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? Cantidad { get; set; }
        [StringLength(1000)]
        public string Comentario { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }
        public int? FacturaMovimientoId { get; set; }

        public virtual Producto Producto { get; set; }
        public virtual FacturaMovimiento FacturaMovimiento { get; set; }

        //public virtual Empleado EmpleadoCrea { get; set; }
        //public virtual Empleado EmpleadoModifica { get; set; }
    }
}
