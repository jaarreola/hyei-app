using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class MarcaHerramientaDTO
    {
        public int MarcaHerramientaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }
    }
}