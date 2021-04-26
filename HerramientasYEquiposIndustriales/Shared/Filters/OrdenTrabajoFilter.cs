using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class OrdenTrabajoFilter
    {
        public int? OrdenTrabajoId { get; set; }
        public int? EmpleadoId { get; set; }
        public string NumeroOrdenTrabajo { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }


        public string NombreCLiente { get; set; }
        public string TelefonoCLiente { get; set; }
        public string RfcCLiente { get; set; }
        public string EstatusBusqueda { get; set; }

        public int Garantia { get; set; }
    }
}
