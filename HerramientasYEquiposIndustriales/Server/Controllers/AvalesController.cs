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
    public class AvalesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AvalesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet("ObtenerAvalesByClienteId/{clienteId}")]
        public async Task<ActionResult<List<AvalDTO>>> ObtenerAvalesByClienteId(int clienteId)
        {
            try
            {
                var avales = await context.Aval.Include(x => x.Cliente).Where(x => x.ClienteId == clienteId).ToListAsync();
                if (avales == null) { return NotFound(); }

                return mapper.Map<List<AvalDTO>>(avales);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Cotización. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<AvalDTO>> PostAval([FromBody] AvalDTO avalCreacion)
        {
            try
            {
                var aval = mapper.Map<Aval>(avalCreacion);
                context.Aval.Add(aval);
                await context.SaveChangesAsync();
                var dto = mapper.Map<AvalDTO>(aval);
                //return new CreatedAtRouteResult("ObtenerAval", new { id = aval.AvalId }, dto);
                return mapper.Map<AvalDTO>(aval);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el cliente. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AvalDTO>> PutAval(int id, [FromBody] AvalDTO avalMod)
        {
            try
            {
                if (!ExisteAval(id)) { return NotFound(); }

                var aval = mapper.Map<Aval>(avalMod);
                aval.AvalId = id;
                context.Entry(aval).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información de la Referencia. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpGet("TieneAvalesCorrectosByClienteId/{clienteId}")]
        public async Task<ActionResult<bool>> TieneAvalesCorrectosByClienteId(int clienteId)
        {
            try
            {
                if ((await context.Aval.Where(x => x.ClienteId == clienteId && x.Vencido != true).ToListAsync()).Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del aval. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        private bool ExisteAval(int id)
        {
            return context.Aval.Any(x => x.AvalId == id);
        }
    }
}
