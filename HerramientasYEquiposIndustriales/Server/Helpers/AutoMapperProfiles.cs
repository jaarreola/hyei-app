using AutoMapper;
using HerramientasYEquiposIndustriales.Shared.DTOs;
using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasYEquiposIndustriales.Server.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Puesto, PuestoDTO>().ReverseMap();
            CreateMap<Puesto, PuestoCreacionDTO>().ReverseMap();
            CreateMap<Puesto, PuestoComboDTO>().ReverseMap();

            CreateMap<Empleado, EmpleadoDTO>().ReverseMap();
            CreateMap<Empleado, EmpleadoCreacionDTO>().ReverseMap();

            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<Cliente, ClienteCreacionDTO>().ReverseMap();

            CreateMap<Producto, ProductoDTO>().ReverseMap();

            CreateMap<Marca, MarcaDTO>().ReverseMap();
            CreateMap<MarcaHerramienta, MarcaHerramientaDTO>().ReverseMap();

            CreateMap<OrdenTrabajo, OrdenTrabajoDTO>().ReverseMap();
            CreateMap<OrdenTrabajoDetalle, OrdenTrabajoDetalleDTO>().ReverseMap();

            CreateMap<Movimiento, MovimientoDTO>().ReverseMap();
            CreateMap<FacturaMovimiento, FacturaMovimientoDTO>().ReverseMap();

            CreateMap<Cotizacion, CotizacionDTO>().ReverseMap();
            CreateMap<CotizacionDetalle, CotizacionDetalleDTO>().ReverseMap();

            CreateMap<EstatusOT, EstatusOTDTO>().ReverseMap();
            CreateMap<EstatusOTFlujo, EstatusOTFlujoDTO>().ReverseMap();

            CreateMap<Configuraciones, ConfiguracionesDTO>().ReverseMap();

            CreateMap<HistorialPreciosProductos, HistorialPreciosProductosDTO>().ReverseMap();
        }
    }
}
