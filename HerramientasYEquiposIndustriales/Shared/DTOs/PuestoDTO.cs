using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class PuestoDTO
    {
        public int PuestoId { get; set; }
        [Required]
        [StringLength(80)]
        public string Nombre { get; set; }
        public bool EsAdministrador { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
    }
}
