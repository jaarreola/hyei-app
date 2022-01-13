using System;
using System.ComponentModel.DataAnnotations;
using HerramientasYEquiposIndustriales.Shared.Models;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class AvalDTO
    {
        public int AvalId { get; set; }
        public string NombreAval { get; set; }
        public string DireccionAval { get; set; }
        public string TelefonoAval { get; set; }
        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
