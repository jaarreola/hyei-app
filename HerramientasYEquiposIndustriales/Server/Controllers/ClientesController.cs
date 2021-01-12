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

                return new CreatedAtRouteResult("ObtenerCliente", new { id = cliente.ClienteId }, dto);
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
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientesFilter([FromQuery] FiltrosCliente filtrosCliente)
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
                    (x.EsFrecuente == filtrosCliente.EsFrecuente || (filtrosCliente.Todos))
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


        public class FiltrosCliente
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public string Correo { get; set; }
            public string RFC { get; set; }
            public bool EsFrecuente { get; set; }
            public bool Todos { get; set; }
        }
    }
}
