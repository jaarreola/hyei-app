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
    public class InventariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public InventariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoDTO>>> GetInventarios()
        {
            try
            {
                var Inventarios = await context.Movimientos.ToListAsync();
                return mapper.Map<List<MovimientoDTO>>(Inventarios);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Movimientos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerMovimiento")]
        public async Task<ActionResult<MovimientoDTO>> GetMovimiento(int id)
        {
            try
            {
                var Movimiento = await context.Movimientos.FirstOrDefaultAsync(x => x.MovimientoId == id);

                if (Movimiento == null) { return NotFound(); }

                var dto = mapper.Map<MovimientoDTO>(Movimiento);

                return dto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<MovimientoDTO>> PostMovimiento([FromBody] MovimientoDTO MovimientoCreacionDTO)
        {
            try
            {
                var Movimiento = mapper.Map<Movimiento>(MovimientoCreacionDTO);
                Movimiento.FechaRegistro = DateTime.Now;

                context.Movimientos.Add(Movimiento);

                await context.SaveChangesAsync();

                var dto = mapper.Map<MovimientoDTO>(Movimiento);

                return new CreatedAtRouteResult("ObtenerMovimiento", new { id = Movimiento.MovimientoId }, dto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovimientoDTO>> PutMovimiento(int id, [FromBody] MovimientoDTO MovimientoModificacionDTO)
        {
            try
            {
                if (!MovimientoExists(id)) { return NotFound(); }

                var Movimiento = mapper.Map<Movimiento>(MovimientoModificacionDTO);

                Movimiento.MovimientoId = id;
                Movimiento.FechaUltimaModificacion = DateTime.Now;

                context.Entry(Movimiento).State = EntityState.Modified;
                context.Entry(Movimiento).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovimiento(int id)
        {
            try
            {
                if (!MovimientoExists(id)) { return NotFound(); }

                context.Movimientos.Remove(new Movimiento() { MovimientoId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool MovimientoExists(int id)
        {
            return context.Movimientos.Any(x => x.MovimientoId == id);
        }


        [HttpGet]
        [Route("/api/[Controller]/ObtenerMovimientosFilter")]
        public async Task<ActionResult<IEnumerable<MovimientoDTO>>> ObtenerMovimientosFilter([FromQuery] MovimientoFilter filtro)
        {
            try
            {
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);

                var Productos = await context.Movimientos.Include(x => x.FacturaMovimiento).Include(x => x.Producto).Where(x =>
                    ((x.EsEntrada == true && filtro.TipoEntrada == 1) || (x.EsSalida == true && filtro.TipoEntrada == -1) || filtro.TipoEntrada == 0) &&
                    (x.FacturaMovimiento.Factura.Contains(filtro.Factura) || filtro.Factura == null || filtro.Factura == String.Empty) &&
                    (x.Producto.NoParte.Contains(filtro.NoParte) || filtro.NoParte == null || filtro.NoParte == String.Empty) &&
                    ((x.FechaRegistro.Value.Date >= filtro.FechaInicio && x.FechaRegistro.Value.Date < filtro.FechaFin) || filtro.FechaInicio == null || filtro.FechaFin == null)
                ).ToListAsync();
                return mapper.Map<List<MovimientoDTO>>(Productos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }
    }
}
