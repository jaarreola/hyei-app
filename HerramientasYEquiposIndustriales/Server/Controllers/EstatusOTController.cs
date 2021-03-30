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
    public class EstatusOTController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EstatusOTController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("GetEstatusOT")]
        public async Task<ActionResult<List<EstatusOTDTO>>> GetEstatusOT()
        {
            try
            {
                var result = await context.EstatusOTs.OrderBy(x => x.Posicion).ToListAsync();
                return mapper.Map<List<EstatusOTDTO>>(result);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetEstatusOTByDescripcion/{descripcion}")]
        public async Task<ActionResult<EstatusOTDTO>> GetEstatusOTByDescripcion(String descripcion)
        {
            try
            {
                var result = await context.EstatusOTs.Where(x => x.Descripcion == descripcion).FirstOrDefaultAsync();
                return mapper.Map<EstatusOTDTO>(result);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetSiguienteEstatusOTByEstatusOTId/{estatusId}")]
        public async Task<ActionResult<EstatusOTDTO>> GetSiguienteEstatusOTByEstatusOTId(int estatusId)
        {
            try
            {
                var estatusActual = await context.EstatusOTs.FirstOrDefaultAsync(x => x.EstatusOTId == estatusId);
                var estatusSig = await context.EstatusOTs.FirstOrDefaultAsync(x => x.Posicion == (estatusActual.Posicion + 1) && estatusActual.Posicion != -1);
                if (estatusSig == null) { return NotFound(); }
                return mapper.Map<EstatusOTDTO>(estatusSig);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }
    }
}
