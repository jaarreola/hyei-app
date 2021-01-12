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
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EmpleadosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoDTO>>> GetEmpleados()
        {
            try
            {
                var empleados = await context.Empleados.Include(x => x.Puesto).ToListAsync();
                return mapper.Map<List<EmpleadoDTO>>(empleados);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerEmpleado")]
        public async Task<ActionResult<EmpleadoDTO>> GetEmpleado(int id)
        {
            try
            {
                var empleado = await context.Empleados.Include(x => x.Puesto).FirstOrDefaultAsync(x => x.EmpleadoId == id);

                if (empleado == null) return NotFound();

                var dto = mapper.Map<EmpleadoDTO>(empleado);

                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del empleado. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerEmpleadosFilter")]
        public async Task<ActionResult<IEnumerable<EmpleadoDTO>>> GetEmpleadoFilter([FromQuery] FiltrosEmpleado filtrosEmpleado)
        {
            try
            {
                var empleados = await context.Empleados.Include(x => x.Puesto).Where(x => 
                    (x.Nombre.Contains(filtrosEmpleado.Nombre) || filtrosEmpleado.Nombre == null) && 
                    (x.Direccion.Contains(filtrosEmpleado.Direccion) || filtrosEmpleado.Direccion == null) && 
                    (x.Activo == filtrosEmpleado.Activo || (filtrosEmpleado.Todos))
                ).ToListAsync();
                return mapper.Map<List<EmpleadoDTO>>(empleados);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerNumeroEmpleado")]
        public ActionResult<string> GetNumeroEmpleado()
        {
            try
            {
                var numeroEmpleado = (context.Empleados.Max(x => x.EmpleadoId) + 1).ToString("D4");
                return $"{DateTime.Now.Year}-{numeroEmpleado}"; ;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<EmpleadoDTO>> PostEmpleado(EmpleadoDTO empleadoCreacionDTO)
        {
            try
            {
                var empleado = mapper.Map<Empleado>(empleadoCreacionDTO);
                empleado.FechaRegistro = DateTime.Now;
                empleado.Activo = true;

                context.Empleados.Add(empleado);
                await context.SaveChangesAsync();

                var dto = mapper.Map<EmpleadoDTO>(empleado);

                return new CreatedAtRouteResult("ObtenerEmpleado", new { id = empleado.EmpleadoId }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el empleado. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<EmpleadoDTO>> PutEmpleado(int id, [FromBody] EmpleadoDTO empleadoModificacionDTO)
        {
            try
            {
                if (!EmpleadoExists(id)) { return NotFound(); }

                var empleado = mapper.Map<Empleado>(empleadoModificacionDTO);

                empleado.EmpleadoId = id;
                empleado.FechaUltimaModificacion = DateTime.Now;

                if (!empleado.Activo)
                    empleado.FechaBaja = DateTime.Now;
                else
                    empleado.FechaBaja = null;

                context.Entry(empleado).State = EntityState.Modified;
                context.Entry(empleado).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del empleado. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Empleado>> DeleteEmpleado(int id)
        {
            try
            {
                if (!EmpleadoExists(id)) { return NotFound(); }

                context.Empleados.Remove(new Empleado() { EmpleadoId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el empleado. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool EmpleadoExists(int id)
        {
            return context.Empleados.Any(x => x.EmpleadoId == id);
        }

        public class FiltrosEmpleado
        {
            public string Nombre { get; set; }
            public string Direccion { get; set; }
            public bool Activo { get; set; }
            public bool Todos { get; set; }
        }
    }
}
