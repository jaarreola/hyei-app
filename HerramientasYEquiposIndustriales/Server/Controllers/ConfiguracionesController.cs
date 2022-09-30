using AutoMapper;
using HerramientasYEquiposIndustriales.Server.Constants;
using HerramientasYEquiposIndustriales.Server.Context;
using HerramientasYEquiposIndustriales.Shared.DTOs;
using HerramientasYEquiposIndustriales.Shared.Filters;
using HerramientasYEquiposIndustriales.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasYEquiposIndustriales.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ConfiguracionesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("GetIpRutaReportes")]
        public async Task<ActionResult<ConfiguracionesDTO>> GetIpRutaReportes()
        {
            try
            {
                var configuracion = await context.Configuraciones.FirstOrDefaultAsync(x => x.Tipo == "ipRutaReportes");
                if (configuracion == null) { return NotFound(); }

                return mapper.Map<ConfiguracionesDTO>(configuracion);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener valores de Configuraciones. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetIpRutaReporteCotizacion")]
        public async Task<ActionResult<ConfiguracionesDTO>> GetIpRutaReporteCotizacion()
        {
            try
            {
                var configuracion = await context.Configuraciones.FirstOrDefaultAsync(x => x.Tipo == "ipReporteCotizacion");
                if (configuracion == null) { return NotFound(); }

                return mapper.Map<ConfiguracionesDTO>(configuracion);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener valores de Configuraciones. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetIpRutaReporteInventario")]
        public async Task<ActionResult<ConfiguracionesDTO>> GetIpRutaReporteInventario()
        {
            try
            {
                var configuracion = await context.Configuraciones.FirstOrDefaultAsync(x => x.Tipo == "ipReporteInventario");
                if (configuracion == null) { return NotFound(); }

                return mapper.Map<ConfiguracionesDTO>(configuracion);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener valores de Configuraciones. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetIpRutaReporteContrato")]
        public async Task<ActionResult<ConfiguracionesDTO>> GetIpRutaReporteContrato()
        {
            try
            {
                var configuracion = await context.Configuraciones.FirstOrDefaultAsync(x => x.Tipo == "ipReporteContrato");
                if (configuracion == null) { return NotFound(); }

                return mapper.Map<ConfiguracionesDTO>(configuracion);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener valores de Configuraciones. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }
    }
}
