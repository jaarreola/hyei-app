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
    public class MarcasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MarcasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaDTO>>> GetMarcas()
        {
            try
            {
                var Marcas = await context.Marcas.ToListAsync();
                return mapper.Map<List<MarcaDTO>>(Marcas);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerMarca")]
        public async Task<ActionResult<MarcaDTO>> GetMarca(int id)
        {
            try
            {
                var Marca = await context.Marcas.FirstOrDefaultAsync(x => x.MarcaId == id);

                if (Marca == null) return NotFound();

                var dto = mapper.Map<MarcaDTO>(Marca);

                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Marca. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetMarcaByNombre/{nombre}")]
        public async Task<ActionResult<MarcaDTO>> GetMarcaByNombre(string nombre)
        {
            try
            {
                var marca = await context.Marcas.FirstOrDefaultAsync(x => x.Descripcion == nombre);
                if (marca == null) { return NotFound(); }

                var dto = mapper.Map<MarcaDTO>(marca);
                return dto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de la Cotización. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerMarcasFilter")]
        public async Task<ActionResult<IEnumerable<MarcaDTO>>> GetMarcaFilter([FromQuery] MarcaFilter filtrosMarca)
        {
            try
            {
                var Marcas = await context.Marcas.Where(x =>
                    (x.Descripcion.Contains(filtrosMarca.Descripcion) || filtrosMarca.Descripcion == null)
                ).ToListAsync();
                return mapper.Map<List<MarcaDTO>>(Marcas);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Marcas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<MarcaDTO>> PostMarca(MarcaDTO MarcaCreacionDTO)
        {
            try
            {
                var Marca = mapper.Map<Marca>(MarcaCreacionDTO);
               
                context.Marcas.Add(Marca);
                await context.SaveChangesAsync();

                var dto = mapper.Map<MarcaDTO>(Marca);

                return new CreatedAtRouteResult("ObtenerMarca", new { id = Marca.MarcaId }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Marca. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<MarcaDTO>> PutMarca(int id, [FromBody] MarcaDTO MarcaModificacionDTO)
        {
            try
            {
                if (!MarcaExists(id)) { return NotFound(); }

                var Marca = mapper.Map<Marca>(MarcaModificacionDTO);

                Marca.MarcaId = id;
                
                context.Entry(Marca).State = EntityState.Modified;
                
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del Marca. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Marca>> DeleteMarca(int id)
        {
            try
            {
                if (!MarcaExists(id)) { return NotFound(); }

                context.Marcas.Remove(new Marca() { MarcaId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el Marca. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool MarcaExists(int id)
        {
            return context.Marcas.Any(x => x.MarcaId == id);
        }
    }
}
