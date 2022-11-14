using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class HistorialTrabajoEmpleadoDTO
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
        public DateTime? FechaRegistroOtd { get; set; }
        public int? EmpleadoCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? EmpleadoModificacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaFinaliacion { get; set; }
        public bool TieneCotizacion { get; set; }
        public String Comentarios { get; set; }
        public bool? Revision { get; set; }
        public bool? Reparacion { get; set; }


        public int EstatusOTFlujoId { get; set; }
        public int EstatusOTId { get; set; }
        [StringLength(100)]
        public bool? Terminado { get; set; }
        public String Ubicacion { get; set; }
        public DateTime? FechaRegistro { get; set; }


        [StringLength(100)]
        public string Descripcion { get; set; }
        public int Posicion { get; set; }


        public int EmpleadoId { get; set; }
        [StringLength(10)]
        public string NumeroEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Nss { get; set; }
        public string Curp { get; set; }
        public int PuestoId { get; set; }
        public bool Activo { get; set; }
        public bool PuedeEditar { get; set; }
        

        public decimal costoReparacion { get; set; }
    }
}
