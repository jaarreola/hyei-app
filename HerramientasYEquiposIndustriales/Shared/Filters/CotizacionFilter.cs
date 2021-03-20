using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class CotizacionFilter
    {
        public int? OrdenTrabajoId { get; set; }
        public int? EmpleadoId { get; set; }
        public string NumeroOrdenTrabajo { get; set; }
    }
}
