using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class MovimientoFilter
    {
        //TipoEntrada 1=Entrada, -1=Salida, 0=Ambos 
        public int TipoEntrada { get; set; }
        public string Factura { get; set; }
        public string NoParte { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
