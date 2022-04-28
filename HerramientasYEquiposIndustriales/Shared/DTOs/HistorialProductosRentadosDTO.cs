using System;
using System.ComponentModel.DataAnnotations;
using HerramientasYEquiposIndustriales.Shared.Models;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class HistorialProductosRentadosDTO
    {
        public int ProdcutoTiendaId { get; set; }
        public string Sku { get; set; }
        public string NombreHerramienta { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int FolioProductoTienda { get; set; }
        public int RentasId { get; set; }
        public int VecesRentado { get; set; }
        public DateTime? FechaInicioRenta { get; set; }
        public DateTime? FechaFinRenta { get; set; }
        public DateTime? FechaEntrega { get; set; }

    }
}
