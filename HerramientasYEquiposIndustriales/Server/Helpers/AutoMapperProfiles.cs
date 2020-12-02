using AutoMapper;
using HerramientasYEquiposIndustriales.Shared.DTOs;
using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasYEquiposIndustriales.Server.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Puesto, PuestoDTO>().ReverseMap();
            CreateMap<Puesto, PuestoCreacionDTO>().ReverseMap();

            CreateMap<Empleado, EmpleadoDTO>().ReverseMap();
            CreateMap<Empleado, EmpleadoCreacionDTO>().ReverseMap();
        }
    }
}
