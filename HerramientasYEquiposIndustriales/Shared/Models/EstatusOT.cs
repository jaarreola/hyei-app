using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class EstatusOT
    {
        public int EstatusOTId { get; set; }
        [StringLength(100)]
        public string Descripcion { get; set; }
        public int Posicion { get; set; }
    }
}
