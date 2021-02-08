using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class OrdenTrabajoDetalleDTO
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

        public virtual OrdenTrabajo Ordentrabajo { get; set; }
    }
}