using AutoMapper;
using HerramientasYEquiposIndustriales.Server.Constants;
using HerramientasYEquiposIndustriales.Server.Context;
using HerramientasYEquiposIndustriales.Shared.DTOs;
using HerramientasYEquiposIndustriales.Shared.Filters;
using HerramientasYEquiposIndustriales.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HerramientasYEquiposIndustriales.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CotizacionesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet("GetCotizacionByOrdenTrabajoId/{ordenTrabajoDetalleId}")]
        public async Task<ActionResult<CotizacionDTO>> GetCotizacionByOrdenTrabajoId(int ordenTrabajoDetalleId)
        {
            try
            {
                var cotizacion = await context.Cotizaciones.FirstOrDefaultAsync(x => x.OrdenTrabajoDetalleId == ordenTrabajoDetalleId);
                if (cotizacion == null) { return NotFound(); }

                var dto = mapper.Map<CotizacionDTO>(cotizacion);
                return dto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Cotización. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetOTParaCotizar")]
        public async Task<ActionResult<IEnumerable<OrdenTrabajoDetalleDTO>>> GetOTParaCotizar([FromQuery] OrdenTrabajoFilter filtro)
        {
            try
            {
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;

                var ordenesTrabajo = await context.OrdenTrabajoDetalle.Where(x =>
                    ((x.FechaRegistro.Value.Date >= filtro.FechaInicio && x.FechaRegistro.Value.Date < filtro.FechaFin) || filtro.FechaInicio == null || filtro.FechaFin == null) &&
                    (!x.TieneCotizacion)
                ).OrderBy(x => x.FechaRegistro).ToListAsync();
                return mapper.Map<List<OrdenTrabajoDetalleDTO>>(ordenesTrabajo);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de las ordenes de trabajo. \n{CommonConstant.MSG_ERROR_FIN}\n" + ex.Message);
            }
        }


        [HttpPut("ActualizaCotizacion/{cotizacionId}")]
        public async Task<ActionResult<CotizacionDTO>> ActualizaCotizacion(int cotizacionId, [FromBody] CotizacionDTO cotizacionDto)
        {
            try
            {
                if (!CotizacionExists(cotizacionId)) { return NotFound(); }

                var cotizacion = mapper.Map<Cotizacion>(cotizacionDto);

                cotizacion.CotizacionId = cotizacionId;
                cotizacion.FechaUltimaModificacion = DateTime.Now;

                context.Entry(cotizacion).State = EntityState.Modified;
                context.Entry(cotizacion).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información de la cotización. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPut("ActualizaCotizacionDetalle/{cotizacionDetalleId}")]
        public async Task<ActionResult<CotizacionDetalleDTO>> ActualizaCotizacionDetalle(int cotizacionDetalleId, [FromBody] CotizacionDetalleDTO cotizacionDetalleDto)
        {
            try
            {
                if (!CotizacionDetalleExists(cotizacionDetalleId)) { return NotFound(); }

                var cotizacionDetalle = mapper.Map<CotizacionDetalle>(cotizacionDetalleDto);

                cotizacionDetalle.CotizacionDetalleId = cotizacionDetalleId;
                cotizacionDetalle.FechaUltimaModificacion = DateTime.Now;

                context.Entry(cotizacionDetalle).State = EntityState.Modified;
                context.Entry(cotizacionDetalle).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del cotización. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpDelete("DeleteCotizacionDetalle/{id}")]
        public async Task<ActionResult> DeleteCotizacionDetalle(int id)
        {
            try
            {
                if (!CotizacionDetalleExists(id)) { return NotFound(); }

                context.CotizacionDetalles.Remove(new CotizacionDetalle() { CotizacionDetalleId = id });
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el detalle de la cotización. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        //public async Task<ActionResult<bool>> PostSaveCotizacion(List<object> datos)

        [HttpPost("SaveCotizacion")]
        public bool PostSaveCotizacion(List<object> datos)
        {
            try
            {
                Cotizacion cotizacion = new Cotizacion();
                List<CotizacionDetalle> detalles = new List<CotizacionDetalle>();
                CotizacionDetalle detalle;

                if (datos.Count == 2)
                {
                    DateTime fecha = DateTime.Now;
                    cotizacion = mapper.Map<Cotizacion>(JsonConvert.DeserializeObject<CotizacionDTO>(datos[0].ToString()));
                    if (cotizacion.CotizacionId == 0)
                        cotizacion.FechaRegistro = fecha;
                    else
                        cotizacion.FechaUltimaModificacion = fecha;

                    foreach (CotizacionDetalle m in (JsonConvert.DeserializeObject<List<CotizacionDetalle>>(datos[1].ToString())))
                    {
                        detalle = mapper.Map<CotizacionDetalle>(m);
                        detalle.FechaRegistro = fecha;
                        detalles.Add(detalle);
                    }

                    using var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
                    if (cotizacion.CotizacionId == 0)
                    {
                        context.Cotizaciones.Add(cotizacion);
                        context.SaveChanges();

                        //actualizamos la OT indicando que ya tiene cotizacion
                        var ot = context.OrdenTrabajoDetalle.FirstOrDefault(x => x.OrdenTrabajoDetalleId == cotizacion.OrdenTrabajoDetalleId);
                        ot.TieneCotizacion = true;
                        context.Entry(ot).State = EntityState.Modified;
                        context.Entry(ot).Property(x => x.FechaRegistro).IsModified = false;
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Entry(cotizacion).State = EntityState.Modified;
                        context.Entry(cotizacion).Property(x => x.FechaRegistro).IsModified = false;
                        context.SaveChanges();
                    }

                    var idCotizacionCreacion = context.Cotizaciones.FirstOrDefaultAsync(x => x.OrdenTrabajoDetalleId == cotizacion.OrdenTrabajoDetalleId).Result.CotizacionId;
                    foreach (var m in detalles)
                    {
                        if (m.CotizacionDetalleId == 0)
                        {
                            m.CotizacionId = idCotizacionCreacion;
                            m.Producto = null;

                            context.CotizacionDetalles.Add(m);
                            context.SaveChanges();
                        }
                    }
                    scope.Complete();
                }

                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

                //return StatusCode(StatusCodes.Status500InternalServerError,
                //    $"{CommonConstant.MSG_ERROR_INICIO} " +
                //    $"al crear la Cotización. \n{CommonConstant.MSG_ERROR_FIN}");
                return false;
            }
        }


        private bool CotizacionDetalleExists(int id)
        {
            return context.CotizacionDetalles.Any(x => x.CotizacionDetalleId == id);
        }

        private bool CotizacionExists(int id)
        {
            return context.Cotizaciones.Any(x => x.CotizacionId == id);
        }
    }
}
