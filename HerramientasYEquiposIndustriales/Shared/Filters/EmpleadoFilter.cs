using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class EmpleadoFilter
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public bool? Activo { get; set; }
    }
}
