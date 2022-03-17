using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Aval
    {
        public int AvalId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public int ClienteId { get; set; }
        public bool Vencido { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
