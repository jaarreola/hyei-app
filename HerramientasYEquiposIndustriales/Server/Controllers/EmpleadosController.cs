using AutoMapper;
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
            var empleados = await context.Empleados.Include(x => x.Puesto).ToListAsync();
            return mapper.Map<List<EmpleadoDTO>>(empleados);
        }

        [HttpGet("{id}", Name = "ObtenerEmpleado")]
        public async Task<ActionResult<EmpleadoDTO>> GetEmpleado(int id)
        {
            var empleado = await context.Empleados.Include(x => x.Puesto).FirstOrDefaultAsync(x => x.EmpleadoId == id);

            if (empleado == null) return NotFound();

            var dto = mapper.Map<EmpleadoDTO>(empleado);

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<EmpleadoDTO>> PostEmpleado([FromBody] EmpleadoCreacionDTO empleadoCreacionDTO)
        {
            var empleado = mapper.Map<Empleado>(empleadoCreacionDTO);
            empleado.FechaRegistro = DateTime.Now;
            empleado.Activo = true;

            context.Empleados.Add(empleado);
            await context.SaveChangesAsync();

            var dto = mapper.Map<EmpleadoDTO>(empleado);

            return new CreatedAtRouteResult("ObtenerEmpleado", new { id = empleado.EmpleadoId }, dto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmpleadoDTO>> PutEmpleado(int id, [FromBody] EmpleadoCreacionDTO empleadoModificacionDTO)
        {

            if (!EmpleadoExists(id)) { return NotFound(); }

            var empleado = mapper.Map<Empleado>(empleadoModificacionDTO);

            empleado.EmpleadoId = id;
            empleado.FechaUltimaModificacion = DateTime.Now;

            if (!empleado.Activo)
                empleado.FechaBaja = DateTime.Now;

            context.Entry(empleado).State = EntityState.Modified;
            context.Entry(empleado).Property(x => x.FechaRegistro).IsModified = false;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Empleado>> DeleteEmpleado(int id)
        {

            if (!EmpleadoExists(id)) { return NotFound(); }

            context.Empleados.Remove(new Empleado() { EmpleadoId = id });

            await context.SaveChangesAsync();

            return NoContent();
        }


        private bool EmpleadoExists(int id)
        {
            return context.Empleados.Any(x => x.EmpleadoId == id);
        }
    }
}
