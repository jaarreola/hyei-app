using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class MarcaDTO
    {
        public int MarcaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }
    }
}