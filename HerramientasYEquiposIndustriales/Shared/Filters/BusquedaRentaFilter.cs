using System;
using System.Collections.Generic;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.Filters
{
    public class BusquedaRentaFilter
    {
        public int RentasId { get; set; }
        public DateTime? FechaInicioRenta { get; set; }
        public DateTime? FechaFinRenta { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string RFC { get; set; }
        public string Sku { get; set; }
        public int ProductoTiendaId { get; set; }

        public int EstatusRenta { get; set; }  //1 - ACTIVAS, 2 - VENCIDAS, 3 - ENTREGADOS
    }
}
