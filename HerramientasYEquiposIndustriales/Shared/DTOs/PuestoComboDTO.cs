using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class PuestoComboDTO
    {
        public int PuestoId { get; set; }
        [Required]
        [StringLength(80)]
        public string Nombre { get; set; }
    }
}
