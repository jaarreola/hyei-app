using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HerramientasYEquiposIndustriales.Server.Context;
using HerramientasYEquiposIndustriales.Shared.DTOs;
using HerramientasYEquiposIndustriales.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HerramientasYEquiposIndustriales.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuestosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PuestosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PuestoDTO>>> GetPuestos()
        {
            var puestos = await context.Puestos.ToListAsync();
            return mapper.Map<List<PuestoDTO>>(puestos);
        }

        [HttpGet("{id}", Name = "ObtenerPuesto")]
        public async Task<ActionResult<PuestoDTO>> GetPuesto(int id)
        {
            var puesto = await context.Puestos.FirstOrDefaultAsync(x => x.PuestoId == id);

            if (puesto == null) return NotFound();

            var dto = mapper.Map<PuestoDTO>(puesto);
            
            return dto;

        }

        [HttpPost]
        public async Task<ActionResult<PuestoDTO>> PostPuesto([FromBody] PuestoCreacionDTO puestoCreacionDTO)
        {
            var puesto = mapper.Map<Puesto>(puestoCreacionDTO);
            puesto.FechaRegistro = DateTime.Now;
            puesto.FechaUltimaModificacion = DateTime.Now;

            context.Puestos.Add(puesto);
            await context.SaveChangesAsync();

            var dto = mapper.Map<PuestoDTO>(puesto);

            return new CreatedAtRouteResult("ObtenerPuesto", new { id = puesto.PuestoId }, puesto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PuestoDTO>> PutPuesto(int id, [FromBody] PuestoCreacionDTO puestoModificacionDTO)
        {

            if (!PuestoExists(id)) { return NotFound(); }

            var puesto = mapper.Map<Puesto>(puestoModificacionDTO);

            puesto.PuestoId = id;
            puesto.FechaUltimaModificacion = DateTime.Now;

            context.Entry(puesto).State = EntityState.Modified;
            context.Entry(puesto).Property(x => x.FechaRegistro).IsModified = false;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Puesto>> DeletePuesto(int id)
        {
            
            if (!PuestoExists(id)) { return NotFound(); }

            context.Puestos.Remove(new Puesto() { PuestoId = id });
            
            await context.SaveChangesAsync();

            return NoContent();
        }



        private bool PuestoExists(int id)
        {
            return context.Puestos.Any(x => x.PuestoId == id);
        }

    }
}
