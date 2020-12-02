using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Puesto
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
