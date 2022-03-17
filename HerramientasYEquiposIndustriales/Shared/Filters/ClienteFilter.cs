using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class ClienteFilter
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string RFC { get; set; }
        public bool EsFrecuente { get; set; }
        public bool EsProblema { get; set; }
        public bool Todos { get; set; }
        public int PuedeRentar { get; set; }
        public int Activo { get; set; }
    }
}
