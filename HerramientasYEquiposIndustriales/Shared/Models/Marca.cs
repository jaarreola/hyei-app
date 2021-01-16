using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class Marca
    {
        public int MarcaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }
    }
}