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
    public class MarcasTiendaController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MarcasTiendaController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcasProductosTiendaDTO>>> GetMarcasTienda()
        {
            try
            {
                var Marcas = await context.MarcasProductosTienda.OrderBy(x => x.Descripcion).ToListAsync();
                return mapper.Map<List<MarcasProductosTiendaDTO>>(Marcas);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de MarcasProductosTienda. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("GetMarcasHerramientas")]
        public async Task<ActionResult<IEnumerable<MarcaHerramientaDTO>>> GetMarcasHerramientas()
        {
            try
            {
                var Marcas = await context.MarcaHerramientas.OrderBy(x => x.Descripcion).ToListAsync();
                return mapper.Map<List<MarcaHerramientaDTO>>(Marcas);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Marcas para las herramientas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("{id}", Name = "ObtenerMarcaTienda")]
        public async Task<ActionResult<MarcasProductosTiendaDTO>> GetMarca(int id)
        {
            try
            {
                var Marca = await context.MarcasProductosTienda.FirstOrDefaultAsync(x => x.MarcasProductosTiendaId == id);

                if (Marca == null) return NotFound();

                var dto = mapper.Map<MarcasProductosTiendaDTO>(Marca);

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
        public async Task<ActionResult<MarcasProductosTiendaDTO>> GetMarcaByNombre(string nombre)
        {
            try
            {
                var marca = await context.MarcasProductosTienda.FirstOrDefaultAsync(x => x.Descripcion == nombre);
                if (marca == null) { return NotFound(); }

                var dto = mapper.Map<MarcasProductosTiendaDTO>(marca);
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
        public async Task<ActionResult<IEnumerable<MarcasProductosTiendaDTO>>> GetMarcaFilter([FromQuery] MarcaFilter filtrosMarca)
        {
            try
            {
                var Marcas = await context.MarcasProductosTienda.Where(x =>
                    (x.Descripcion.Contains(filtrosMarca.Descripcion) || filtrosMarca.Descripcion == null)
                ).ToListAsync();
                return mapper.Map<List<MarcasProductosTiendaDTO>>(Marcas);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los MarcasProductosTienda. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<MarcasProductosTiendaDTO>> PostMarca(MarcasProductosTiendaDTO MarcaCreacionDTO)
        {
            try
            {
                var Marca = mapper.Map<MarcasProductosTienda>(MarcaCreacionDTO);

                context.MarcasProductosTienda.Add(Marca);
                await context.SaveChangesAsync();

                var dto = mapper.Map<MarcasProductosTiendaDTO>(Marca);

                return new CreatedAtRouteResult("ObtenerMarca", new { id = Marca.MarcasProductosTiendaId }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Marca. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost("SaveMarcaHerramienta")]
        public async Task<ActionResult<MarcaHerramientaDTO>> SaveMarcaHerramienta(MarcaHerramientaDTO MarcaCreacionDTO)
        {
            try
            {
                var Marca = mapper.Map<MarcaHerramienta>(MarcaCreacionDTO);
                Marca.Descripcion = Marca.Descripcion.ToUpper();

                if (MarcaHerramientaExists(Marca.Descripcion)) { return NotFound(); }

                context.MarcaHerramientas.Add(Marca);
                await context.SaveChangesAsync();

                return mapper.Map<MarcaHerramientaDTO>(Marca);
                //var dto = mapper.Map<MarcaHerramientaDTO>(Marca);
                //return new CreatedAtRouteResult("ObtenerMarcaHerramienta", new { id = Marca.MarcaHerramientaId }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Marca. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<MarcasProductosTiendaDTO>> PutMarca(int id, [FromBody] MarcasProductosTiendaDTO MarcaModificacionDTO)
        {
            try
            {
                if (!MarcaExists(id)) { return NotFound(); }

                var Marca = mapper.Map<MarcasProductosTienda>(MarcaModificacionDTO);

                Marca.MarcasProductosTiendaId = id;

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
        public async Task<ActionResult<MarcasProductosTienda>> DeleteMarca(int id)
        {
            try
            {
                if (!MarcaExists(id)) { return NotFound(); }

                context.MarcasProductosTienda.Remove(new MarcasProductosTienda() { MarcasProductosTiendaId = id });

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
            return context.MarcasProductosTienda.Any(x => x.MarcasProductosTiendaId == id);
        }

        private bool MarcaHerramientaExists(String nombre)
        {
            return context.MarcaHerramientas.Any(x => x.Descripcion == nombre);
        }
    }
}
