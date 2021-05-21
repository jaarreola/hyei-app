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


        [HttpGet("GetOrdeTrabajoById/{id}")]
        public async Task<ActionResult<OrdenTrabajoDTO>> GetOrdeTrabajoById(int id)
        {
            try
            {
                var OrdenTrabajo = await context.OrdenTrabajo.Include(x => x.Cliente).FirstOrDefaultAsync(x => x.OrdenTrabajoId == id);
                if (OrdenTrabajo == null) return NotFound();

                var dto = mapper.Map<OrdenTrabajoDTO>(OrdenTrabajo);
                return dto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
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
                    _cliente = GetClienteExists(_cliente);

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


        [HttpGet("GetOTs")]
        public async Task<ActionResult<IEnumerable<OrdenTrabajoDetalleConsultaDTO>>> GetOTs([FromQuery] OrdenTrabajoFilter filtro)
        {
            try
            {
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;

                //garantia 1= Local, 2= Fabrica, 3= Sin garantia, 0= Todos

                var consulta = from otd in context.OrdenTrabajoDetalle
                               join ot in context.OrdenTrabajo on otd.OrdenTrabajoId equals ot.OrdenTrabajoId
                               join c in context.Clientes on ot.ClienteId equals c.ClienteId
                               join ef in context.EstatusOTFlujos on otd.OrdenTrabajoDetalleId equals ef.OrdenTrabajoDetalleId into ef1
                               from ef2 in ef1.DefaultIfEmpty()
                               join e in context.EstatusOTs on ef2.EstatusOTId equals e.EstatusOTId into e1
                               from e2 in e1.DefaultIfEmpty()
                               where ef2.Terminado == null &&
                                   ((otd.FechaRegistro.Value.Date >= filtro.FechaInicio && otd.FechaRegistro.Value.Date < filtro.FechaFin) || (filtro.FechaInicio == null && filtro.FechaFin == null)) &&
                                   (otd.NumeroOrdenTrabajo.Contains(filtro.NumeroOrdenTrabajo) || filtro.NumeroOrdenTrabajo == null) &&
                                   (c.Nombre.Contains(filtro.NombreCLiente) || c.Apellido.Contains(filtro.NombreCLiente) || filtro.NombreCLiente == null) &&
                                   (c.Telefono.Contains(filtro.TelefonoCLiente) || filtro.TelefonoCLiente == null) &&
                                   (c.RFC.Contains(filtro.RfcCLiente) || filtro.RfcCLiente == null) &&
                                   ((otd.GarantiaLocal == true && filtro.Garantia == 1) || (otd.GarantiaFabrica == true && filtro.Garantia == 2) || (!otd.GarantiaLocal && !otd.GarantiaFabrica && filtro.Garantia == 3) || filtro.Garantia == 0) &&
                                   (e2.Descripcion == filtro.EstatusBusqueda || (e2.Descripcion == null && (filtro.EstatusBusqueda == "Sin Cotizar" || filtro.EstatusBusqueda == null)) || filtro.EstatusBusqueda == "Todos")
                               select new OrdenTrabajoDetalleConsultaDTO()
                               {
                                   OrdenTrabajoDetalleId = otd.OrdenTrabajoDetalleId,
                                   OrdenTrabajoId = otd.OrdenTrabajoId,
                                   NumeroOrdenTrabajo = otd.NumeroOrdenTrabajo,
                                   NombreHerramienta = otd.NombreHerramienta,
                                   Marca = otd.Marca,
                                   Modelo = otd.Modelo,
                                   NumeroSerie = otd.NumeroSerie,
                                   GarantiaFabrica = otd.GarantiaFabrica,
                                   GarantiaFabricaDetalle = otd.GarantiaFabricaDetalle,
                                   GarantiaLocal = otd.GarantiaLocal,
                                   GarantiaLocalDetalle = otd.GarantiaLocalDetalle,
                                   TiempoGarantia = otd.TiempoGarantia,
                                   FechaRegistro = otd.FechaRegistro,
                                   EmpleadoCreacion = otd.EmpleadoCreacion,
                                   FechaUltimaModificacion = otd.FechaUltimaModificacion,
                                   EmpleadoModificacion = otd.EmpleadoModificacion,
                                   FechaEntrega = otd.FechaEntrega,
                                   FechaFinaliacion = otd.FechaFinaliacion,
                                   TieneCotizacion = otd.TieneCotizacion,
                                   Comentarios = otd.Comentarios,
                                   Ubicacion = ef2.Ubicacion,
                                   ClienteId = c.ClienteId,
                                   Nombre = c.Nombre,
                                   Apellido = c.Apellido,
                                   Telefono = c.Telefono,
                                   Correo = c.Correo,
                                   Direccion = c.Direccion,
                                   RFC = c.RFC,
                                   EsFrecuente = c.EsFrecuente,
                                   EstatusOTFlujoId = ef2.EstatusOTFlujoId,
                                   EstatusOTId = ef2.EstatusOTId,
                                   Terminado = ef2.Terminado,
                                   Descripcion = e2.Descripcion,
                                   Posicion = e2.Posicion
                               };

                var consultaResultado = from con in consulta
                                join cot in (
                                    from cot in context.Cotizaciones
                                    join cd in context.CotizacionDetalles on cot.CotizacionId equals cd.CotizacionId
                                    group new { cot, cd } by new { cot.OrdenTrabajoDetalleId } into r
                                    select new {
                                        r.Key.OrdenTrabajoDetalleId,
                                        costoReparacion = r.Sum(x => x.cd.Cantidad * x.cd.CostoUnitario)
                                    }
                                ) on con.OrdenTrabajoDetalleId equals cot.OrdenTrabajoDetalleId into m1
                                from m2 in m1.DefaultIfEmpty()
                                orderby con.FechaRegistro ascending
                                select new OrdenTrabajoDetalleConsultaDTO()
                                {
                                    OrdenTrabajoDetalleId = con.OrdenTrabajoDetalleId,
                                    OrdenTrabajoId = con.OrdenTrabajoId,
                                    NumeroOrdenTrabajo = con.NumeroOrdenTrabajo,
                                    NombreHerramienta = con.NombreHerramienta,
                                    Marca = con.Marca,
                                    Modelo = con.Modelo,
                                    NumeroSerie = con.NumeroSerie,
                                    GarantiaFabrica = con.GarantiaFabrica,
                                    GarantiaFabricaDetalle = con.GarantiaFabricaDetalle,
                                    GarantiaLocal = con.GarantiaLocal,
                                    GarantiaLocalDetalle = con.GarantiaLocalDetalle,
                                    TiempoGarantia = con.TiempoGarantia,
                                    FechaRegistro = con.FechaRegistro,
                                    EmpleadoCreacion = con.EmpleadoCreacion,
                                    FechaUltimaModificacion = con.FechaUltimaModificacion,
                                    EmpleadoModificacion = con.EmpleadoModificacion,
                                    FechaEntrega = con.FechaEntrega,
                                    FechaFinaliacion = con.FechaFinaliacion,
                                    TieneCotizacion = con.TieneCotizacion,
                                    Comentarios = con.Comentarios,
                                    Ubicacion = con.Ubicacion,
                                    ClienteId = con.ClienteId,
                                    Nombre = con.Nombre,
                                    Apellido = con.Apellido,
                                    Telefono = con.Telefono,
                                    Correo = con.Correo,
                                    Direccion = con.Direccion,
                                    RFC = con.RFC,
                                    EsFrecuente = con.EsFrecuente,
                                    EstatusOTFlujoId = con.EstatusOTFlujoId,
                                    EstatusOTId = con.EstatusOTId,
                                    Terminado = con.Terminado,
                                    Descripcion = con.Descripcion,
                                    Posicion = con.Posicion,
                                    costoReparacion = (decimal)m2.costoReparacion
                                };

                var result = await consultaResultado.ToListAsync();
                var totalResultado = result.Count;
                return result;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de las ordenes de trabajo. \n{CommonConstant.MSG_ERROR_FIN}\n" + ex.Message);
            }
        }


        [HttpGet("GetHistorialOTs/{idOtd}")]
        public async Task<ActionResult<IEnumerable<HistorialOrdenTrabajoDTO>>> GetHistorialOTs(int idOtd)
        {
            try
            {
                var consulta = from ef in context.EstatusOTFlujos
                               join e in context.EstatusOTs on ef.EstatusOTId equals e.EstatusOTId
                               join em in context.Empleados on ef.EmpleadoCreacion equals em.EmpleadoId
                               where ef.OrdenTrabajoDetalleId == idOtd
                               select new HistorialOrdenTrabajoDTO()
                               {
                                   Posicion = e.Posicion,
                                   Descripcion = e.Descripcion,
                                   FechaRegistro = ef.FechaRegistro,
                                   NumeroEmpleado = em.NumeroEmpleado,
                                   Nombre = em.Nombre,
                                   Ubicacion = ef.Ubicacion,
                                   Comentario = ef.Comentario
                               };

                var result = await consulta.OrderBy(x => x.FechaRegistro).ToListAsync();
                var totalResultado = result.Count;
                return result;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de las ordenes de trabajo. \n{CommonConstant.MSG_ERROR_FIN}\n" + ex.Message);
            }
        }


        private Cliente GetClienteExists(Cliente cliente)
        {
            var clienteE = context.Clientes.FirstOrDefault(x => x.Nombre == cliente.Nombre && x.Apellido == cliente.Apellido && x.Telefono == cliente.Telefono && x.Correo == cliente.Correo && x.RFC == cliente.RFC && x.Direccion == cliente.Direccion);
            if (clienteE == null)
                clienteE = cliente;
            return clienteE;
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
