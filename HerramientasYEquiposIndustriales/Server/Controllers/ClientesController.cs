using AutoMapper;
using HerramientasYEquiposIndustriales.Server.Constants;
using HerramientasYEquiposIndustriales.Server.Context;
using HerramientasYEquiposIndustriales.Shared.DTOs;
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
    }
}
