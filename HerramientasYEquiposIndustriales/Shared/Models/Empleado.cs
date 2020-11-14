using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        public string NumeroEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaBaja { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }

        public virtual Puesto Puesto { get; set; }
    }
}
