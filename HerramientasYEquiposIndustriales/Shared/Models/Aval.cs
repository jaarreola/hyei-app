using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Aval
    {
        public int AvalId { get; set; }
        public string NombreAval { get; set; }
        public string DireccionAval { get; set; }
        public string TelefonoAval { get; set; }
        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
