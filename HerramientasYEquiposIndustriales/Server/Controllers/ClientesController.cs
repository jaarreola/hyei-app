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
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ClientesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
        {
            try
            {
                var clientes = await context.Clientes.ToListAsync();
                return mapper.Map<List<ClienteDTO>>(clientes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de clientes. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerCliente")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.ClienteId == id);

                if (cliente == null) { return NotFound(); }

                var dto = mapper.Map<ClienteDTO>(cliente);

                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del cliente. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet]
        [Route("/api/[Controller]/Busqueda")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetBusquedaEmpleados([FromBody] ClienteFilter filter)
        {
            try
            {
                var clientes = await context.Clientes
                    .Where(x => x.Nombre.Contains(string.IsNullOrEmpty(filter.Nombre) ? x.Nombre : filter.Nombre)
                        & x.Apellido.Contains(string.IsNullOrEmpty(filter.Apellido) ? x.Apellido : filter.Apellido)
                        & x.Telefono.Contains(string.IsNullOrEmpty(filter.Telefono) ? x.Telefono : filter.Telefono)
                        & x.Correo.Contains(string.IsNullOrEmpty(filter.Correo) ? x.Correo : filter.Correo)
                        & x.RFC.Contains(string.IsNullOrEmpty(filter.RFC) ? x.RFC : filter.RFC))
                    .ToListAsync();

                return mapper.Map<List<ClienteDTO>>(clientes);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de clientes. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> PostCliente([FromBody] ClienteCreacionDTO clienteCreacionDTO)
        {
            try
            {
                var cliente = mapper.Map<Cliente>(clienteCreacionDTO);
                cliente.FechaRegistro = DateTime.Now;

                context.Clientes.Add(cliente);
                await context.SaveChangesAsync();
                var dto = mapper.Map<ClienteDTO>(cliente);

                //return new CreatedAtRouteResult("ObtenerCliente", new { id = cliente.ClienteId }, dto);
                return mapper.Map<ClienteDTO>(cliente);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el cliente. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteDTO>> PutCliente(int id, [FromBody] ClienteCreacionDTO clienteModificacionDTO)
        {
            try
            {
                if (!ClienteExists(id)) { return NotFound(); }

                var cliente = mapper.Map<Cliente>(clienteModificacionDTO);

                cliente.ClienteId = id;
                cliente.FechaUltimaModificacion = DateTime.Now;

                context.Entry(cliente).State = EntityState.Modified;
                context.Entry(cliente).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del cliente. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            try
            {
                if (!ClienteExists(id)) { return NotFound(); }

                context.Clientes.Remove(new Cliente() { ClienteId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el cliente. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool ClienteExists(int id)
        {
            return context.Clientes.Any(x => x.ClienteId == id);
        }


        [HttpGet("ObtenerClientesFilter")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientesFilter([FromQuery] ClienteFilter filtrosCliente)
        {
            try
            {
                var clientes = await context.Clientes.Where(x =>
                    (x.Nombre.Contains(filtrosCliente.Nombre) || filtrosCliente.Nombre == null) &&
                    (x.Apellido.Contains(filtrosCliente.Apellido) || filtrosCliente.Apellido == null) &&
                    (x.Direccion.Contains(filtrosCliente.Direccion) || filtrosCliente.Direccion == null) &&
                    (x.Telefono.Contains(filtrosCliente.Telefono) || filtrosCliente.Telefono == null) &&
                    (x.Correo.Contains(filtrosCliente.Correo) || filtrosCliente.Correo == null) &&
                    (x.RFC.Contains(filtrosCliente.RFC) || filtrosCliente.RFC == null) &&
                    (x.EsProblema == filtrosCliente.EsProblema || filtrosCliente.EsProblema == false) &&
                    (x.EsFrecuente == filtrosCliente.EsFrecuente || (filtrosCliente.Todos)) &&
                    ((filtrosCliente.PuedeRentar == 1 && x.PuedeRentar == true) || (filtrosCliente.PuedeRentar == 2 && (x.PuedeRentar ?? false) == false) || filtrosCliente.PuedeRentar == 0)
                ).ToListAsync();

                return mapper.Map<List<ClienteDTO>>(clientes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerClientesRentaFilter")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> ObtenerClientesRentaFilter([FromQuery] ClienteFilter filtrosCliente)
        {
            try
            {
                //var vigencias1 = await context.VigenciaClientesRenta.ToListAsync();

                var vigencias = from v in context.VigenciaClientesRenta
                                group v by new { v.ClienteId } into g
                                select new
                                {
                                    ClienteId = g.Key.ClienteId,
                                    FechaInicio = g.Max(a => a.FechaInicio),
                                    FechaFin = g.Max(a => a.FechaFin)
                                };


                var clientes = await context.Clientes.Where(x =>
                    x.PuedeRentar == true &&
                    (x.Nombre.Contains(filtrosCliente.Nombre) || filtrosCliente.Nombre == null) &&
                    (x.Apellido.Contains(filtrosCliente.Apellido) || filtrosCliente.Apellido == null) &&
                    (x.Direccion.Contains(filtrosCliente.Direccion) || filtrosCliente.Direccion == null) &&
                    (x.Telefono.Contains(filtrosCliente.Telefono) || filtrosCliente.Telefono == null) &&
                    (x.Correo.Contains(filtrosCliente.Correo) || filtrosCliente.Correo == null) &&
                    (x.RFC.Contains(filtrosCliente.RFC) || filtrosCliente.RFC == null) &&
                    (x.EsProblema == filtrosCliente.EsProblema || filtrosCliente.EsProblema == false) &&
                    (x.EsFrecuente == filtrosCliente.EsFrecuente || (filtrosCliente.Todos)) &&
                    ((filtrosCliente.PuedeRentar == 1 && x.PuedeRentar == true) || (filtrosCliente.PuedeRentar == 2 && (x.PuedeRentar ?? false) == false) || filtrosCliente.PuedeRentar == 0)
                ).ToListAsync();


                var query = from c in clientes
                            join v in vigencias on c.ClienteId equals v.ClienteId
                            into RClientes
                            from rC in RClientes.DefaultIfEmpty()
                            where (filtrosCliente.Activo == 1 && (rC != null && (rC.FechaFin >= DateTime.Today))) || (filtrosCliente.Activo == 2 && ((rC != null && (rC.FechaFin < DateTime.Today.AddDays(-1))) || rC == null)) || (filtrosCliente.Activo == 0)
                            select new ClienteDTO
                            {
                                ClienteId = c.ClienteId,
                                Nombre = c.Nombre,
                                Apellido = c.Apellido,
                                Telefono = c.Telefono,
                                Correo = c.Correo,
                                Direccion = c.Direccion,
                                RFC = c.RFC,
                                EsFrecuente = c.EsFrecuente,
                                EsProblema = c.EsProblema,
                                FechaRegistro = c.FechaRegistro,
                                FechaUltimaModificacion = c.FechaUltimaModificacion,
                                PuedeRentar = c.PuedeRentar,
                                ClienteVigenteParaRenta = rC != null && (rC.FechaFin >= DateTime.Today),
                                FechaVencimientoParaRenta = (rC?.FechaFin)
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerClientesSinRentaFilter")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> ObtenerClientesSinRentaFilter([FromQuery] ClienteFilter filtrosCliente)
        {
            try
            {
                var clientes = await context.Clientes.Where(x =>
                    (x.PuedeRentar ?? false) == false &&
                    ((x.Nombre.Contains(filtrosCliente.Nombre) || filtrosCliente.Nombre == null) ||
                    (x.Apellido.Contains(filtrosCliente.Nombre) || filtrosCliente.Nombre == null)) &&
                    (x.Direccion.Contains(filtrosCliente.Direccion) || filtrosCliente.Direccion == null) &&
                    (x.Telefono.Contains(filtrosCliente.Telefono) || filtrosCliente.Telefono == null) &&
                    (x.RFC.Contains(filtrosCliente.RFC) || filtrosCliente.RFC == null) 
                ).ToListAsync();

                return mapper.Map<List<ClienteDTO>>(clientes);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerClientesOTFilter")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientesOTFilter([FromQuery] ClienteFilter filtrosCliente)
        {
            try
            {
                var clientes = await context.Clientes.Where(x =>
                    (x.Nombre.Contains(filtrosCliente.Nombre) && filtrosCliente.Nombre != null) ||
                    (x.Apellido.Contains(filtrosCliente.Apellido) && filtrosCliente.Apellido != null) ||
                    (x.Telefono.Contains(filtrosCliente.Telefono) && filtrosCliente.Telefono != null) ||
                    (x.RFC.Contains(filtrosCliente.RFC) && filtrosCliente.RFC != null)
                ).ToListAsync();

                return mapper.Map<List<ClienteDTO>>(clientes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpGet("ExtenderVigenciaRentaByClienteId/{clienteId}")]
        public async Task<ActionResult<VigenciaClientesRentaDTO>> ExtenderVigenciaRentaByClienteId(int clienteId)
        {
            try
            {
                var vigencias = from v in context.VigenciaClientesRenta
                                where v.ClienteId == clienteId && v.FechaFin >= DateTime.Today
                                group v by new { v.ClienteId } into g
                                select new
                                {
                                    ClienteId = g.Key.ClienteId,
                                    FechaFin = g.Max(a => a.FechaFin)
                                };

                DateTime? nuevaFechaFin;
                if (vigencias.ToList().Count > 0)
                    nuevaFechaFin = vigencias.FirstOrDefaultAsync().Result.FechaFin.Value.AddMonths(6);  //nuevaFechaFin = vigencias.FirstOrDefaultAsync().Result.FechaFin.Value.AddYears(1);
                else
                    nuevaFechaFin = DateTime.Today.AddMonths(6);

                var nuevaVigencia = new VigenciaClientesRenta() 
                {
                    ClienteId = clienteId,
                    FechaInicio = DateTime.Today.Date,
                    FechaFin = nuevaFechaFin
                };
                
                context.VigenciaClientesRenta.Add(nuevaVigencia);
                await context.SaveChangesAsync();
                
                return mapper.Map<VigenciaClientesRentaDTO>(nuevaVigencia);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Cotización. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerClienteRentaByClienteId/{clienteId}")]
        public async Task<ActionResult<ClienteDTO>> ObtenerClienteRentaByClienteId(int clienteId)
        {
            try
            {
                var vigencias = from v in context.VigenciaClientesRenta
                                where v.ClienteId == clienteId
                                group v by new { v.ClienteId } into g
                                select new
                                {
                                    ClienteId = g.Key.ClienteId,
                                    FechaInicio = g.Max(a => a.FechaInicio),
                                    FechaFin = g.Max(a => a.FechaFin)
                                };

                var cliente = await context.Clientes.FirstOrDefaultAsync(x =>
                    //x.PuedeRentar == true &&
                    x.ClienteId == clienteId
                );

                var clienteDto = new ClienteDTO();
                if (cliente != null)
                {
                    clienteDto = mapper.Map<ClienteDTO>(cliente);
                    if(vigencias.ToList().Count>0)
                        clienteDto.FechaVencimientoParaRenta = vigencias.FirstOrDefaultAsync(x => x.ClienteId == clienteId).Result.FechaFin;
                }

                return clienteDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del cliente. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }




        [HttpGet("EsClienteProblemaFilter")]
        public async Task<ActionResult<ClienteDTO>> EsClienteProblemaFilter([FromQuery] ClienteFilter filtrosCliente)
        {
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(x =>
                    ((x.Nombre.Contains(filtrosCliente.Nombre) && filtrosCliente.Nombre != null) ||
                    (x.Apellido.Contains(filtrosCliente.Apellido) && filtrosCliente.Apellido != null)) &&
                    (x.Telefono.Contains(filtrosCliente.Telefono) && filtrosCliente.Telefono != null) &&
                    x.EsProblema == true
                );

                if (cliente == null) { return NotFound(); }

                var dto = mapper.Map<ClienteDTO>(cliente);
                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }
    }
}
