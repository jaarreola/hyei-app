using HerramientasYEquiposIndustriales.Shared.Models;
using System;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class VigenciaClientesRentaDTO
    {
        public int VigenciaClientesRentaId { get; set; }
        public int ClienteId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int EmpleadoRegistro { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
