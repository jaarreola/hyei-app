using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class RentasDetalleDTO
    {
        public int ProductosTiendaId { get; set; }
        public string Sku { get; set; }
        public string Herramienta { get; set; }
        public int RentaId { get; set; }
        public int ProductoTiendaExistenciasId { get; set; }
        public bool Rentado { get; set; }
        public int FolioProductoTienda { get; set; }
        public DateTime? FechaInicioRenta { get; set; }
        public DateTime? FechaFinRenta { get; set; }
        public DateTime? FechaEntregado { get; set; }
        public float? TotalRenta { get; set; }
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

    }
}
