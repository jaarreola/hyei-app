using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class ExistenciasBusquedaDTO
    {
        public int ProductosTiendaId { get; set; }
        public string Sku { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Usados { get; set; }
        public int Nuevos { get; set; }
        public int ParaRenta { get; set; }
        public int ParaVenta { get; set; }
        public int Rentados { get; set; }
        public int Disponibles { get; set; }
        public int Vendidos { get; set; }
        public int NoVendidos { get; set; }
    }
}
