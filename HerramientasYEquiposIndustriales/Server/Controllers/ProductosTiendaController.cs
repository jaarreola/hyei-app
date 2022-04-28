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
using System.Transactions;

namespace HerramientasYEquiposIndustriales.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosTiendaController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductosTiendaController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductosTiendaDTO>>> GetProductos()
        {
            try
            {
                //var Productos = await context.ProductosTienda.Include(x => x.Marca).Include(x => x.Existencias).ToListAsync();
                var Productos = await context.ProductosTienda.Include(x => x.Marca).ToListAsync();
                return mapper.Map<List<ProductosTiendaDTO>>(Productos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de ProductosTiendas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerProductoTienda")]
        public async Task<ActionResult<ProductosTiendaDTO>> GetProducto(int id)
        {
            try
            {
                var Producto = await context.ProductosTienda.Include(x => x.Marca).FirstOrDefaultAsync(x => x.ProductosTiendaId == id);

                if (Producto == null) return NotFound();

                var dto = mapper.Map<ProductosTiendaDTO>(Producto);

                return dto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("GetProductoPorSku/{Sku}")]
        public async Task<ActionResult<ProductosTiendaDTO>> GetProductoPorSku(String Sku)
        {
            try
            {
                var producto = await context.ProductosTienda.FirstOrDefaultAsync(x => x.Sku == Sku);

                if (producto == null) { return NotFound(); }

                var dto = mapper.Map<ProductosTiendaDTO>(producto);

                return dto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerProductosFilter")]
        public async Task<ActionResult<IEnumerable<ProductosTiendaDTO>>> GetProductoFilter([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                //var Productos = await context.ProductosTienda.Include(x => x.Marca).Include(x => x.Existencias).Where(x =>
                var Productos = await context.ProductosTienda.Include(x => x.Marca).Where(x =>
                    (x.Sku.Contains(filtrosProducto.NoParte) || filtrosProducto.NoParte == null) &&
                    (x.Nombre.Contains(filtrosProducto.Nombre) || filtrosProducto.Nombre == null) &&
                    (x.Modelo.Contains(filtrosProducto.Modelo) || filtrosProducto.Modelo == null) &&
                    (x.MarcasProductosTiendaId == filtrosProducto.MarcaId || filtrosProducto.MarcaId == null)
                ).ToListAsync();
                return mapper.Map<List<ProductosTiendaDTO>>(Productos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los ProductosTiendas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        //
        [HttpGet("BuscaProductosFilter")]
        public async Task<ActionResult<IEnumerable<ProductosTiendaDTO>>> BuscaProductosFilter([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Productos = await context.ProductosTienda.Include(x => x.Marca).Where(x =>
                    (x.Sku.Contains(filtrosProducto.NoParte) && filtrosProducto.NoParte != null) ||
                    (x.Nombre.Contains(filtrosProducto.Nombre) && filtrosProducto.Nombre != null) ||
                    (x.Marca.Descripcion.Contains(filtrosProducto.Marca) && filtrosProducto.Marca != null)
                ).ToListAsync();
                return mapper.Map<List<ProductosTiendaDTO>>(Productos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los ProductosTiendas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("BuscaProductoPorNumeroParte")]
        public async Task<ActionResult<ProductosTiendaDTO>> BuscaProductoPorNumeroParte([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Producto = await context.ProductosTienda.Include(x => x.Marca).FirstOrDefaultAsync(x => x.Sku == filtrosProducto.NoParte);
                if (Producto == null)
                    return new ProductosTiendaDTO();
                else
                    return mapper.Map<ProductosTiendaDTO>(Producto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Producto a buscar. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }
        //
        //


        [HttpPost]
        public ActionResult<ProductosTiendaDTO> PostProducto(ProductosTiendaDTO ProductoCreacionDTO)
        {
            try
            {
                var Producto = mapper.Map<ProductosTienda>(ProductoCreacionDTO);
                Producto.FechaRegistro = DateTime.Now;
                ProductosTiendaDTO dto = new ProductosTiendaDTO();

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    context.ProductosTienda.Add(Producto);
                    context.SaveChanges();

                    dto = mapper.Map<ProductosTiendaDTO>(Producto);

                    if (Producto.CostoCompra != 0)
                    {
                        //AJUSTAR PARA EL HISTORIAL DE PRECIOS DEL PRODUCTO
                        HistorialPreciosProductosTienda nuevoPrecio = new HistorialPreciosProductosTienda()
                        {
                            ProductosTiendaId = Producto.ProductosTiendaId,
                            CostoCompra = Producto.CostoCompra,
                            CostoVenta = Producto.CostoVenta,
                            FechaRegistro = Producto.FechaRegistro,
                            EmpleadoCreacion = Producto.EmpleadoCreacion
                        };
                        context.HistorialPreciosProductosTienda.Add(nuevoPrecio);
                        context.SaveChanges();
                    }

                    scope.Complete();
                }
                return new CreatedAtRouteResult("ObtenerProducto", new { id = Producto.ProductosTiendaId }, dto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPut("{id}")]
        public ActionResult<ProductosTiendaDTO> PutProducto(int id, [FromBody] ProductosTiendaDTO ProductoModificacionDTO)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    if (!ProductoExists(id)) { return NotFound(); }

                    var Producto = mapper.Map<ProductosTienda>(ProductoModificacionDTO);

                    Producto.ProductosTiendaId = id;
                    Producto.FechaModificacion = DateTime.Now;

                    context.Entry(Producto).State = EntityState.Modified;
                    context.Entry(Producto).Property(x => x.FechaRegistro).IsModified = false;
                    context.Entry(Producto).Property(x => x.EmpleadoCreacion).IsModified = false;
                    context.Entry(Producto).Property(x => x.EmpleadoBaja).IsModified = false;
                    context.Entry(Producto).Property(x => x.EmpleadoActivo).IsModified = false;

                    context.SaveChanges();

                    //HISTORIAL DE PRECIOS
                    var ultimoPrecio = context.HistorialPreciosProductosTienda.OrderByDescending(x => x.FechaRegistro).FirstOrDefault(x => x.ProductosTiendaId == Producto.ProductosTiendaId);

                    if (Producto.CostoCompra != (ultimoPrecio == null ? 0 : ultimoPrecio.CostoCompra))
                    {
                        HistorialPreciosProductosTienda nuevoPrecio = new HistorialPreciosProductosTienda()
                        {
                            ProductosTiendaId = Producto.ProductosTiendaId,
                            CostoCompra = Producto.CostoCompra,
                            CostoVenta = Producto.CostoVenta,
                            FechaRegistro = Producto.FechaModificacion,
                            EmpleadoCreacion = Producto.EmpleadoModificacion
                        };
                        context.HistorialPreciosProductosTienda.Add(nuevoPrecio);
                        context.SaveChanges();
                    }
                    scope.Complete();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPut("ActivarDesactivarProducto/{id}")]
        public async Task<ActionResult<ProductosTiendaDTO>> ActivarDesactivarProducto(int id, [FromBody] ProductosTiendaDTO ProductoModificacionDTO)
        {
            try
            {
                if (!ProductoExists(id)) { return NotFound(); }

                var Producto = mapper.Map<ProductosTienda>(ProductoModificacionDTO);
                var fecha = DateTime.Now;

                Producto.ProductosTiendaId = id;
                Producto.FechaModificacion = DateTime.Now;

                context.Entry(Producto).State = EntityState.Modified;
                context.Entry(Producto).Property(x => x.FechaRegistro).IsModified = false;
                context.Entry(Producto).Property(x => x.EmpleadoCreacion).IsModified = false;

                if (Producto.FechaBaja == null)
                {
                    Producto.FechaActivo = null;
                    Producto.EmpleadoActivo = null;
                    Producto.FechaBaja = fecha;
                    Producto.EmpleadoBaja = Producto.EmpleadoModificacion;
                }
                else if (Producto.FechaBaja != null)
                {
                    Producto.FechaBaja = null;
                    Producto.EmpleadoBaja = null;
                    Producto.FechaActivo = fecha;
                    Producto.EmpleadoActivo = Producto.EmpleadoModificacion;
                }

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductosTienda>> DeleteProducto(int id)
        {
            try
            {
                if (!ProductoExists(id)) { return NotFound(); }

                context.ProductosTienda.Remove(new ProductosTienda() { ProductosTiendaId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool ProductoExists(int id)
        {
            return context.ProductosTienda.Any(x => x.ProductosTiendaId == id);
        }


        [HttpGet("ObtenerProductoBySku")]
        public async Task<ActionResult<ProductosTiendaDTO>> ObtenerProductoBySku([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Productos = await context.ProductosTienda.Include(x => x.Marca).Where(x => x.Sku.Contains(filtrosProducto.NoParte)).ToListAsync();
                return mapper.Map<ProductosTiendaDTO>(Productos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los ProductosTiendas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerProductoByFilter")]
        public async Task<ActionResult<IEnumerable<ProductosTiendaDTO>>> ObtenerProductoByFilter([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Productos = await context.ProductosTienda.Include(x => x.Marca).Where(x =>
                    (x.Sku.Contains(filtrosProducto.NoParte) && filtrosProducto.NoParte != null) ||
                    (x.Nombre.Contains(filtrosProducto.Nombre) && filtrosProducto.Nombre != null) ||
                    (x.Modelo.Contains(filtrosProducto.Modelo) && filtrosProducto.Modelo != null) ||
                    (x.MarcasProductosTiendaId == filtrosProducto.MarcaId && filtrosProducto.MarcaId != 0)
                ).ToListAsync();
                return mapper.Map<List<ProductosTiendaDTO>>(Productos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los ProductosTiendas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetHistorialPrecioProductoTienda/{ProductosTiendaId}")]
        public async Task<ActionResult<List<HistorialPreciosProductosTiendaDTO>>> GetHistorialPrecioProductoTienda(int ProductosTiendaId)
        {
            try
            {
                var historial = await context.HistorialPreciosProductosTienda.Where(x => x.ProductosTiendaId == ProductosTiendaId).OrderByDescending(x => x.FechaRegistro).ToListAsync();
                if (historial == null)
                    historial = new List<HistorialPreciosProductosTienda>();

                return mapper.Map<List<HistorialPreciosProductosTiendaDTO>>(historial);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del historial de precios. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }
    }
}
