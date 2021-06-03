﻿using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class CostoInventarioDTO
    {
        public int ProductoId { get; set; }
        public String NoParte { get; set; }
        public string Nombre { get; set; }
        public decimal? Entradas { get; set; }
        public decimal? Salidas { get; set; }
        public decimal Existencias { get; set; }
        public decimal CostoCompra { get; set; }
        public decimal CostoVenta { get; set; }

        public decimal TotalCostoCompra { get; set; }
        public decimal TotalCostoVenta { get; set; }

        public decimal CantidadMinimaInventario { get; set; }
        public String Ubicacion { get; set; }


        public bool EsEntrada { get; set; }
        public bool EsSalida { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public String NoEmpleado{ get; set; }
        public String Empleado { get; set; }
    }
}
