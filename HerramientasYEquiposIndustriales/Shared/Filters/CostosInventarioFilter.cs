using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class CostosInventarioFilter
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public string NoParte { get; set; }
        public string NombreParte { get; set; }

        public bool EnExistencia { get; set; }
        public bool SinExistencia { get; set; }
    }
}
