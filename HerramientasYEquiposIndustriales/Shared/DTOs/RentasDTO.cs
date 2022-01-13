using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class RentasDTO
    {
        public int RentaId { get; set; }
        public int ProductoTiendaExistenciasId { get; set; }
        public int ClienteId { get; set; }
        public string DocumentosEnGarantia { get; set; }
        public DateTime? FechaInicioRenta { get; set; }
        public DateTime? FechaFinRenta { get; set; }
        public DateTime? FechaEntrega { get; set; }
        [DataType(DataType.Currency)]
        public float? PrecioRenta { get; set; }
        [DataType(DataType.Currency)]
        public float? PrecioRentaTotal { get; set; }
        [DataType(DataType.Currency)]
        public float? Recargo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int EmpleadoCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int EmpleadoModificacion { get; set; }

        public virtual ProductoTiendaExistencias ProductoTiendaExistencias { get; set; }
    }
}
