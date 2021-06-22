using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class MarcaHerramienta
    {
        public int MarcaHerramientaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }
    }
}