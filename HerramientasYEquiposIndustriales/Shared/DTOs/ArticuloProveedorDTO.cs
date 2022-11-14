using System;
using System.ComponentModel.DataAnnotations;
using HerramientasYEquiposIndustriales.Shared.Models;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class ArticuloProveedorDTO
    {
        public string Proveedor { get; set; }
        public string NoParte { get; set; }
        public string Articulo { get; set; }
        public decimal? Cantidad { get; set; }
    }
}
