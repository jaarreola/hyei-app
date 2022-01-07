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
    public class FlujoEstatusOTController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public FlujoEstatusOTController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("GetEstatusOTByFilter")]
        public async Task<ActionResult<List<EstatusOTFlujoDTO>>> GetEstatusOTByFilter([FromQuery] EstatusFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var result = await context.EstatusOTFlujos.Include(x => x.OrdenTrabajoDetalle).Include(x => x.EstatusOT).Where(x =>
                    (x.EstatusOTId == filtro.EstatusOTId || filtro.EstatusOTId == 0) &&
                    ((x.OrdenTrabajoDetalle.FechaRegistro.Value.Date >= filtro.FechaInicio && x.OrdenTrabajoDetalle.FechaRegistro.Value.Date < filtro.FechaFin) || filtro.FechaInicio == null || filtro.FechaFin == null) &&
                    x.Terminado != true
                ).ToListAsync();
                return mapper.Map<List<EstatusOTFlujoDTO>>(result);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("GetEstatusOTById/{id}")]
        public async Task<ActionResult<EstatusOTFlujoDTO>> GetEstatusOTById(int id)
        {
            try
            {
                var result = await context.EstatusOTFlujos.Include(x => x.OrdenTrabajoDetalle).Where(x => x.EstatusOTFlujoId == id).FirstOrDefaultAsync();
                return mapper.Map<EstatusOTFlujoDTO>(result);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetLastEstatusOTByOtdId/{otdId}")]
        public async Task<ActionResult<EstatusOTFlujoDTO>> GetLastEstatusOTByOtdId(int otdId)
        {
            try
            {
                var result = await context.EstatusOTFlujos.Include(x => x.OrdenTrabajoDetalle).Include(x => x.EstatusOT).Where(x => x.OrdenTrabajoDetalleId == otdId && x.Terminado == null).FirstOrDefaultAsync();
                EstatusOTFlujoDTO res;
                if (result == null)
                {
                    res = new EstatusOTFlujoDTO();
                    res.OrdenTrabajoDetalleId = otdId;
                }   
                else
                    res = mapper.Map<EstatusOTFlujoDTO>(result);

                return res;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost("GuardaFlujoEstatusOTByOT")]
        public async Task<ActionResult<EstatusOTFlujoDTO>> GuardaFlujoEstatusOTByOT(EstatusOTFlujoDTO estatusFlujoCreacionDTO)
        {
            try
            {
                var estatus = mapper.Map<EstatusOTFlujo>(estatusFlujoCreacionDTO);
                estatus.FechaRegistro = DateTime.Now;

                context.EstatusOTFlujos.Add(estatus);
                await context.SaveChangesAsync();

                var dto = mapper.Map<EstatusOTFlujoDTO>(estatus);

                return dto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Marca. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost("GuardaSiguienteEstatus")]
        public ActionResult<bool> GuardaSiguienteEstatus(List<object> datos)
        {
            bool result = false;
            try
            {
                EstatusOTFlujo estatusActual;
                EstatusOTFlujo estatusSiguiente;
                Empleado empleado;
                bool cancelaOt;

                int sigEstatusSinAutorizacion = 0;

                if (datos.Count == 4)
                {
                    DateTime fecha = DateTime.Now;
                    estatusActual = mapper.Map<EstatusOTFlujo>(JsonConvert.DeserializeObject<EstatusOTFlujoDTO>(datos[0].ToString()));
                    empleado = mapper.Map<Empleado>(JsonConvert.DeserializeObject<EmpleadoDTO>(datos[1].ToString()));
                    cancelaOt = !datos[2].ToString().Equals("False");
                    sigEstatusSinAutorizacion = int.Parse(datos[3].ToString());

                    estatusActual.Terminado = true;
                    estatusSiguiente = new EstatusOTFlujo()
                    {
                        EmpleadoCreacion = empleado.EmpleadoId,
                        EstatusOT = null,
                        EstatusOTId = SiguienteEstatusOT(sigEstatusSinAutorizacion == 0 ? estatusActual.EstatusOTId : sigEstatusSinAutorizacion, cancelaOt),
                        FechaRegistro = fecha,
                        OrdenTrabajoDetalleId = estatusActual.OrdenTrabajoDetalleId,
                        OrdenTrabajoDetalle = null,
                        Ubicacion = estatusActual.Ubicacion,
                        Comentario = estatusActual.Comentario
                    };

                    if (estatusSiguiente.EstatusOTId != 0)
                    {
                        using var scope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });

                        if(estatusActual.EstatusOTFlujoId != 0)
                        {
                            context.Entry(estatusActual).State = EntityState.Modified;
                            context.Entry(estatusActual).Property(x => x.FechaRegistro).IsModified = false;
                            context.Entry(estatusActual).Property(x => x.Ubicacion).IsModified = false;
                            context.Entry(estatusActual).Property(x => x.Comentario).IsModified = false;
                            context.SaveChanges();
                        }

                        context.EstatusOTFlujos.Add(estatusSiguiente);
                        context.SaveChanges();

                        scope.Complete();

                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al cambiar el estatus. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private int SiguienteEstatusOT(int estatusIdActual, bool cancelarOt)
        {
            int posicionSig = 0;
            int estatusIdSiguiente = 0;
            try
            {
                var estatusActual = context.EstatusOTs.FirstOrDefault(x => x.EstatusOTId == estatusIdActual);
                posicionSig = cancelarOt ? -1 : (estatusActual.Posicion == -1 ? 1 : estatusActual.Posicion + 1);
                var estatusSiguiente = context.EstatusOTs.FirstOrDefault(x => x.Posicion == posicionSig);
                if (estatusSiguiente == null) { return estatusIdActual; }

                estatusIdSiguiente = mapper.Map<EstatusOTDTO>(estatusSiguiente).EstatusOTId;
                return estatusIdSiguiente;
            }
            catch (Exception)
            {
                return estatusIdSiguiente;
            }
        }
    }
}
