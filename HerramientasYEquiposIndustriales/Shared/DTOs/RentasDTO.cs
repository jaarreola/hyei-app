using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class RentasDTO
    {
        public int RentasId { get; set; }
        public int ProductoTiendaExistenciasId { get; set; }
        public int ClienteId { get; set; }
        public string DocumentosEnGarantia { get; set; }
        public DateTime? FechaInicioRenta { get; set; }
        public DateTime? FechaFinRenta { get; set; }
        public DateTime? FechaEntrega { get; set; }


        public int? CantidadDias { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoDia { get; set; }
        public int? CantidadSemanas { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoSemana { get; set; }
        public int? CantidadQuincenas { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoQuincena { get; set; }
        public int? CantidadMeses { get; set; }
        [DataType(DataType.Currency)]
        public float? CostoMes { get; set; }

        [DataType(DataType.Currency)]
        public float? TotalRenta { get; set; }
        public int? TotalHorasRenta { get; set; }
        [DataType(DataType.Currency)]
        public float? Recargo { get; set; }
        [DataType(DataType.Currency)]
        public float? TotalConRecargo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int EmpleadoCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int EmpleadoModificacion { get; set; }
        public bool? Generada { get; set; }

        public virtual ProductoTiendaExistencias ProductoTiendaExistencias { get; set; }
        

        public bool ChkDia { get; set; }
        public bool ChkSemana { get; set; }
        public bool ChkQuincena { get; set; }
        public bool ChkMes { get; set; }

        public virtual ProductosTienda ProductoTienda { get; set; }
    }
}
