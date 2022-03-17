using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HerramientasYEquiposIndustriales.Server.Constants;
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
            try
            {
                var puestos = await context.Puestos.ToListAsync();
                return mapper.Map<List<PuestoDTO>>(puestos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de puestos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("PuestosCombo")]
        public async Task<ActionResult<IEnumerable<PuestoComboDTO>>> GetPuestosCombo()
        {
            try
            {
                var puestos = await context.Puestos.ToListAsync();
                return mapper.Map<List<PuestoComboDTO>>(puestos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de puestos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerPuesto")]
        public async Task<ActionResult<PuestoDTO>> GetPuesto(int id)
        {
            try
            {
                var puesto = await context.Puestos.FirstOrDefaultAsync(x => x.PuestoId == id);

                if (puesto == null) return NotFound();

                var dto = mapper.Map<PuestoDTO>(puesto);

                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del puesto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PuestoDTO>> PostPuesto([FromBody] PuestoCreacionDTO puestoCreacionDTO)
        {
            try
            {
                var puesto = mapper.Map<Puesto>(puestoCreacionDTO);
                puesto.FechaRegistro = DateTime.Now;

                context.Puestos.Add(puesto);
                await context.SaveChangesAsync();

                var dto = mapper.Map<PuestoDTO>(puesto);

                return new CreatedAtRouteResult("ObtenerPuesto", new { id = puesto.PuestoId }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el puesto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PuestoDTO>> PutPuesto(int id, [FromBody] PuestoCreacionDTO puestoModificacionDTO)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del puesto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Puesto>> DeletePuesto(int id)
        {
            try
            {
                if (!PuestoExists(id)) { return NotFound(); }

                context.Puestos.Remove(new Puesto() { PuestoId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el puesto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool PuestoExists(int id)
        {
            return context.Puestos.Any(x => x.PuestoId == id);
        }

    }
}
