using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class OrdenTrabajoDetalleConsultaDTO
    {
        public int OrdenTrabajoDetalleId { get; set; }
        public int OrdenTrabajoId { get; set; }
        [StringLength(10)]
        public string NumeroOrdenTrabajo { get; set; }
        [StringLength(100)]
        public string NombreHerramienta { get; set; }
        [StringLength(100)]
        public string? Marca { get; set; }
        [StringLength(100)]
        public string? Modelo { get; set; }
        [StringLength(100)]
        public string? NumeroSerie { get; set; }
        public bool GarantiaFabrica { get; set; }
        [StringLength(500)]
        public string? GarantiaFabricaDetalle { get; set; }
        public bool GarantiaLocal { get; set; }
        [StringLength(500)]
        public string? GarantiaLocalDetalle { get; set; }
        [StringLength(500)]
        public string? TiempoGarantia { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaFinaliacion { get; set; }
        public bool TieneCotizacion { get; set; }
        public String Comentarios { get; set; }
        public String Ubicacion { get; set; }


        public int ClienteId { get; set; }
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
        [StringLength(40)]
        public string Apellido { get; set; }
        [StringLength(10)]
        public string Telefono { get; set; }
        [StringLength(40)]
        public string Correo { get; set; }
        [StringLength(150)]
        public string Direccion { get; set; }
        [StringLength(13)]
        public string RFC { get; set; }
        public bool? EsFrecuente { get; set; }


        public int EstatusOTFlujoId { get; set; }
        public int EstatusOTId { get; set; }
        [StringLength(100)]
        public bool? Terminado { get; set; }


        [StringLength(100)]
        public string Descripcion { get; set; }
        public int Posicion { get; set; }
    }
}
