using System;


namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class VigenciaClientesRenta
    {
        public int VigenciaClientesRentaId { get; set; }
        public int ClienteId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int EmpleadoRegistro { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
