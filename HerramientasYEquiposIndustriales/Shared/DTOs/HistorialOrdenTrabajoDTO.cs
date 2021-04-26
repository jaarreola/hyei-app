using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class HistorialOrdenTrabajoDTO
    {
        public int EstatusOTFlujoId { get; set; }
        public int OrdenTrabajoDetalleId { get; set; }
        public int EstatusOTId { get; set; }
        [StringLength(100)]
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public bool? Terminado { get; set; }
        [StringLength(1000)]
        public string Ubicacion { get; set; }
        [StringLength(5000)]
        public string Comentario { get; set; }

        
        [StringLength(100)]
        public string Descripcion { get; set; }
        public int Posicion { get; set; }


        public int EmpleadoId { get; set; }
        [Required]
        [StringLength(10)]
        public string NumeroEmpleado { get; set; }
        [StringLength(80)]
        public string Nombre { get; set; }
        [StringLength(150)]
        public string Direccion { get; set; }
        [StringLength(10)]
        public string Telefono { get; set; }
        [StringLength(15)]
        public string Nss { get; set; }
        [StringLength(20)]
        public string Curp { get; set; }
        public int PuestoId { get; set; }
        public bool Activo { get; set; }
        public bool PuedeEditar { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        [StringLength(500)]
        public string MotivoBaja { get; set; }
    }
}
