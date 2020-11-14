using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Puesto
    {
        public int PuestoId { get; set; }
        public string Nombre { get; set; }
        public bool EsAdministrador { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
    }
}
