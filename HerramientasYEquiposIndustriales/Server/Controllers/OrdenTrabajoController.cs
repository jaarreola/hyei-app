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
    public class OrdenTrabajoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OrdenTrabajoController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenTrabajoDTO>>> GetOrdenesTrabajo()
        {
            try
            {
                var ordenesTrabajo = await context.OrdenTrabajo.Include(x => x.Cliente).ToListAsync();
                return mapper.Map<List<OrdenTrabajoDTO>>(ordenesTrabajo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("GetOrdenesTrabajoDetalle")]
        public async Task<ActionResult<IEnumerable<OrdenTrabajoDetalleDTO>>> GetOrdenesTrabajoDetalle()
        {
            try
            {
                var ordenesTrabajo = await context.OrdenTrabajoDetalle.Include(x => x.OrdenTrabajo).ToListAsync();
                return mapper.Map<List<OrdenTrabajoDetalleDTO>>(ordenesTrabajo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpGet("{id}", Name = "ObtenerOrdentrabajo")]
        public async Task<ActionResult<OrdenTrabajoDTO>> GetOrdeTrabajo(int id)
        {
            try
            {
                var OrdenTrabajo = await context.OrdenTrabajo.Include(x => x.Cliente).FirstOrDefaultAsync(x => x.OrdenTrabajoId == id);

                if (OrdenTrabajo == null) return NotFound();

                var dto = mapper.Map<OrdenTrabajoDTO>(OrdenTrabajo);

                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Orden de Trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{numeroOrdenTrabajo}", Name = "ObtenerOrdentrabajoDetalle")]
        public async Task<ActionResult<OrdenTrabajoDetalleDTO>> GetOrdeTrabajoDetalle(string numeroOrdenTrabajo)
        {
            try
            {
                var OrdenTrabajoDetalle = await context.OrdenTrabajoDetalle.Include(x => x.OrdenTrabajo).FirstOrDefaultAsync(x => x.NumeroOrdenTrabajo == numeroOrdenTrabajo);

                if (OrdenTrabajoDetalle == null) return NotFound();

                var dto = mapper.Map<OrdenTrabajoDetalleDTO>(OrdenTrabajoDetalle);

                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Orden de Trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpGet("ObtenerOrdenesTrabajoFilter")]
        public async Task<ActionResult<IEnumerable<OrdenTrabajoDTO>>> GetOrdenesTrabajoFilter([FromQuery] OrdenTrabajoFilter filtrosOrdenTrabajo)
        {
            try
            {
                var OrdenTrabajo = await context.OrdenTrabajo.Include(x => x.Cliente).Where(x =>
                    (x.OrdenTrabajoId == filtrosOrdenTrabajo.OrdenTrabajoId || filtrosOrdenTrabajo.OrdenTrabajoId == null)
                ).ToListAsync();
                return mapper.Map<List<OrdenTrabajoDTO>>(OrdenTrabajo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Orden de Trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("ObtenerOrdenesTrabajoDetalleFilter")]
        public async Task<ActionResult<IEnumerable<OrdenTrabajoDetalleDTO>>> GetOrdenesTrabajoDetalleFilter([FromQuery] OrdenTrabajoFilter filtrosOrdenTrabajoDetalle)
        {
            try
            {
                var OrdenTrabajoDetalle = await context.OrdenTrabajoDetalle.Include(x => x.OrdenTrabajo).Where(x =>
                    (x.NumeroOrdenTrabajo == filtrosOrdenTrabajoDetalle.NumeroOrdenTrabajo || filtrosOrdenTrabajoDetalle.NumeroOrdenTrabajo == string.Empty)
                ).ToListAsync();
                return mapper.Map<List<OrdenTrabajoDetalleDTO>>(OrdenTrabajoDetalle);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Orden de Trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<OrdenTrabajoDTO>> PostOrdenTrabajo(OrdenTrabajoDTO OrdenTrabajoCreacionDTO)
        {
            try
            {
                var OrdenTrabajo = mapper.Map<OrdenTrabajo>(OrdenTrabajoCreacionDTO);
                OrdenTrabajo.FechaRegistro = DateTime.Now;

                context.OrdenTrabajo.Add(OrdenTrabajo);
                await context.SaveChangesAsync();

                var dto = mapper.Map<OrdenTrabajoDTO>(OrdenTrabajo);

                return new CreatedAtRouteResult("ObtenerOrdenTrabajo", new { id = OrdenTrabajo.OrdenTrabajoId }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear la Orden de Trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPost("OrdenTrabajoDetalleCreacion")]
        public async Task<ActionResult<OrdenTrabajoDTO>> PostOrdenTrabajoDetalle(OrdenTrabajoDetalleDTO OrdenTrabajoDetalleCreacionDTO)
        {
            try
            {
                var OrdenTrabajoDetalle = mapper.Map<OrdenTrabajoDetalle>(OrdenTrabajoDetalleCreacionDTO);
                OrdenTrabajoDetalle.FechaRegistro = DateTime.Now;

                context.OrdenTrabajoDetalle.Add(OrdenTrabajoDetalle);
                await context.SaveChangesAsync();

                var dto = mapper.Map<OrdenTrabajoDetalleDTO>(OrdenTrabajoDetalle);

                return new CreatedAtRouteResult("ObtenerOrdenTrabajoDetalle", new { id = OrdenTrabajoDetalle.NumeroOrdenTrabajo }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear la Orden de Trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<OrdenTrabajoDTO>> PutOrdenTrabajo(int id, [FromBody] OrdenTrabajoDTO OrdenTrabajoModificacionDTO)
        {
            try
            {
                if (!OrdenTrabajoExists(id)) { return NotFound(); }

                var OrdenTrabajo = mapper.Map<OrdenTrabajo>(OrdenTrabajoModificacionDTO);

                OrdenTrabajo.OrdenTrabajoId = id;
                OrdenTrabajo.FechaUltimaModificacion = DateTime.Now;

                context.Entry(OrdenTrabajo).State = EntityState.Modified;
                context.Entry(OrdenTrabajo).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del OrdenTrabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPut("OrdenTrabajoDetalle")]
        public async Task<ActionResult<OrdenTrabajoDTO>> PutOrdenTrabajoDetalle([FromBody] OrdenTrabajoDetalleDTO OrdenTrabajoDetalleModificacionDTO)
        {
            try
            {
                if (!OrdenTrabajoDetalleExists(OrdenTrabajoDetalleModificacionDTO.NumeroOrdenTrabajo)) { return NotFound(); }

                var OrdenTrabajoDetalle = mapper.Map<OrdenTrabajoDetalle>(OrdenTrabajoDetalleModificacionDTO);

                OrdenTrabajoDetalle.FechaUltimaModificacion = DateTime.Now;

                context.Entry(OrdenTrabajoDetalle).State = EntityState.Modified;
                context.Entry(OrdenTrabajoDetalle).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del OrdenTrabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost("SaveOrdenTrabajo")]
        public ActionResult<bool> PostSaveOrdenTrabajo(List<object> datos)
        {
            try
            {
                List<OrdenTrabajoDetalle> _ordenesTrabajoDetalle = new List<OrdenTrabajoDetalle>();
                Cliente _cliente = new Cliente();
                OrdenTrabajo _ordenTrabajo = new OrdenTrabajo();

                if (datos.Count == 3)
                {
                    _cliente = mapper.Map<Cliente>(JsonConvert.DeserializeObject<ClienteDTO>(datos[0].ToString()));
                    _cliente.FechaRegistro = DateTime.Now;
                    _cliente.EsFrecuente = false;
                    _ordenTrabajo = mapper.Map<OrdenTrabajo>(JsonConvert.DeserializeObject<OrdenTrabajoDTO>(datos[1].ToString()));
                    foreach (OrdenTrabajoDetalleDTO otd in (JsonConvert.DeserializeObject<List<OrdenTrabajoDetalleDTO>>(datos[2].ToString())))
                    {
                        _ordenesTrabajoDetalle.Add(mapper.Map<OrdenTrabajoDetalle>(otd));
                    }
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    if (_cliente.ClienteId == 0)
                    {
                        context.Clientes.Add(_cliente);
                        context.SaveChanges();
                    }

                    _ordenTrabajo.ClienteId = _cliente.ClienteId;
                    _ordenTrabajo.FechaRegistro = DateTime.Now;
                    context.OrdenTrabajo.Add(_ordenTrabajo);
                    context.SaveChanges();

                    foreach (var otd in _ordenesTrabajoDetalle)
                    {
                        otd.OrdenTrabajoId = _ordenTrabajo.OrdenTrabajoId;
                        otd.EmpleadoCreacion = _ordenTrabajo.EmpleadoCreacion;
                        otd.FechaRegistro = DateTime.Now;
                        context.OrdenTrabajoDetalle.Add(otd);
                        context.SaveChanges();
                    }
                    //context.OrdenTrabajoDetalle.AddRange(_ordenesTrabajoDetalle);
                    //context.SaveChanges();

                    scope.Complete();
                }

                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear la Orden de trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetUltimoConsecutivoNumeroOT")]
        public ActionResult<int> GetUltimoConsecutivoNumeroOT()
        {
            try
            {
                var ordenTrabajoDetalle = context.OrdenTrabajoDetalle.OrderByDescending(x => x.FechaRegistro).FirstOrDefault();
                int ultimoConsecutivoOT = 1;

                if (ordenTrabajoDetalle != null)
                {
                    if (Convert.ToInt32(ordenTrabajoDetalle.NumeroOrdenTrabajo.Split("-")[0]) < DateTime.Now.Year)
                        ultimoConsecutivoOT = 1;
                    else
                        ultimoConsecutivoOT = Convert.ToInt32(ordenTrabajoDetalle.NumeroOrdenTrabajo.Split("-")[1]);
                }

                return ultimoConsecutivoOT;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<OrdenTrabajo>> DeleteOrdenTrabajo(int id)
        {
            try
            {
                if (!OrdenTrabajoExists(id)) { return NotFound(); }

                context.OrdenTrabajo.Remove(new OrdenTrabajo() { OrdenTrabajoId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar la Orden de Trabajo. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool OrdenTrabajoExists(int id)
        {
            return context.OrdenTrabajo.Any(x => x.OrdenTrabajoId == id);
        }

        private bool OrdenTrabajoDetalleExists(string numeroOrdentrabajo)
        {
            return context.OrdenTrabajoDetalle.Any(x => x.NumeroOrdenTrabajo == numeroOrdentrabajo);
        }
    }
}
