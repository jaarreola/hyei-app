using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class ExistenciasTiendaFilter
    {
        public string Sku { get; set; }
        public int ProductoTiendaId { get; set; }
        public int TipoExistencia { get; set; } //Nuevo = 1, Usado = 2, Ambos = 0
        public int TipoNegocio { get; set; }  //Para renta = 1, Para venta = 2, Ambos = 0
        public int TipoDispocicion { get; set; }  //Rentados = 1, Disponibles = 2, Ambos = 0
        public int TipoVenta { get; set; }  //Vendidos = 1, No Vendidos = 2, Ambos = 0
    }
}
