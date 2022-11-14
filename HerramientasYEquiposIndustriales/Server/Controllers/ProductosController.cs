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
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetProductos()
        {
            try
            {
                var Productos = await context.Productos.Include(x => x.Marca).ToListAsync();
                return mapper.Map<List<ProductoDTO>>(Productos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerProducto")]
        public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
        {
            try
            {
                var Producto = await context.Productos.Include(x => x.Marca).FirstOrDefaultAsync(x => x.ProductoId == id);

                if (Producto == null) return NotFound();

                var dto = mapper.Map<ProductoDTO>(Producto);

                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("GetProductoPorNoParte/{noParte}")]
        public async Task<ActionResult<ProductoDTO>> GetProductoPorNoParte(String noParte)
        {
            try
            {
                var producto = await context.Productos.FirstOrDefaultAsync(x => x.NoParte == noParte);

                if (producto == null) { return NotFound(); }

                var dto = mapper.Map<ProductoDTO>(producto);

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
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetProductoFilter([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Productos = await context.Productos.Include(x => x.Marca).Where(x =>
                    (x.NoParte.Contains(filtrosProducto.NoParte) || filtrosProducto.NoParte == null) &&
                    (x.Nombre.Contains(filtrosProducto.Nombre) || filtrosProducto.Nombre == null) &&
                    (x.Modelo.Contains(filtrosProducto.Modelo) || filtrosProducto.Modelo == null) &&
                    (x.MarcaId == filtrosProducto.MarcaId || filtrosProducto.MarcaId == null)
                ).ToListAsync();
                return mapper.Map<List<ProductoDTO>>(Productos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        //
        [HttpGet("BuscaProductosFilter")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> BuscaProductosFilter([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Productos = await context.Productos.Include(x => x.Marca).Where(x =>
                    (x.NoParte.Contains(filtrosProducto.NoParte) && filtrosProducto.NoParte != null) ||
                    (x.Nombre.Contains(filtrosProducto.Nombre) && filtrosProducto.Nombre != null) ||
                    (x.Marca.Descripcion.Contains(filtrosProducto.Marca) && filtrosProducto.Marca != null)
                ).ToListAsync();
                return mapper.Map<List<ProductoDTO>>(Productos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("BuscaProductoPorNumeroParte")]
        public async Task<ActionResult<ProductoDTO>> BuscaProductoPorNumeroParte([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Producto = await context.Productos.Include(x => x.Marca).FirstOrDefaultAsync(x => x.NoParte == filtrosProducto.NoParte);
                if (Producto == null)
                    return new ProductoDTO();
                else
                    return mapper.Map<ProductoDTO>(Producto);
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
        public ActionResult<ProductoDTO> PostProducto(ProductoDTO ProductoCreacionDTO)
        {
            try
            {
                var Producto = mapper.Map<Producto>(ProductoCreacionDTO);
                Producto.FechaRegistro = DateTime.Now;
                if ((Producto.CostoCompra ?? 0) != 0)
                    Producto.FechaModificacionPrecio = DateTime.Now;

                ProductoDTO dto = new ProductoDTO();

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    context.Productos.Add(Producto);
                    context.SaveChanges();

                    dto = mapper.Map<ProductoDTO>(Producto);

                    if ((Producto.CostoCompra ?? 0) != 0)
                    {
                        HistorialPreciosProductos nuevoPrecio = new HistorialPreciosProductos()
                        {
                            ProductoId = Producto.ProductoId,
                            CostoCompra = Producto.CostoCompra,
                            CostoVenta = Producto.CostoVenta,
                            FechaRegistro = Producto.FechaRegistro,
                            EmpleadoCreacion = Producto.EmpleadoCreacion
                        };
                        context.HistorialPreciosProductos.Add(nuevoPrecio);
                        context.SaveChanges();
                    }

                    scope.Complete();
                }
                return new CreatedAtRouteResult("ObtenerProducto", new { id = Producto.ProductoId }, dto);
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
        public ActionResult<ProductoDTO> PutProducto(int id, [FromBody] ProductoDTO ProductoModificacionDTO)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    if (!ProductoExists(id)) { return NotFound(); }

                    var Producto = mapper.Map<Producto>(ProductoModificacionDTO);

                    Producto.ProductoId = id;
                    Producto.FechaUltimaModificacion = DateTime.Now;
                    
                    context.Entry(Producto).State = EntityState.Modified;
                    context.Entry(Producto).Property(x => x.FechaRegistro).IsModified = false;
                    context.Entry(Producto).Property(x => x.EmpleadoCreacion).IsModified = false;
                    context.Entry(Producto).Property(x => x.EmpleadoBaja).IsModified = false;
                    context.Entry(Producto).Property(x => x.EmpleadoActivo).IsModified = false;
                    context.SaveChanges();

                    var ultimoPrecio = context.HistorialPreciosProductos.OrderByDescending(x => x.FechaRegistro).FirstOrDefault(x => x.ProductoId == Producto.ProductoId);
                        
                    if (Producto.CostoCompra != (ultimoPrecio == null ? 0 : ultimoPrecio.CostoCompra) || Producto.CostoVenta != (ultimoPrecio == null ? 0 : ultimoPrecio.CostoVenta))
                    {
                        HistorialPreciosProductos nuevoPrecio = new HistorialPreciosProductos()
                        {
                            ProductoId = Producto.ProductoId,
                            CostoCompra = Producto.CostoCompra,
                            CostoVenta = Producto.CostoVenta,
                            FechaRegistro = Producto.FechaUltimaModificacion,
                            EmpleadoCreacion = Producto.EmpleadoModificacion
                        };
                        context.HistorialPreciosProductos.Add(nuevoPrecio);
                        context.SaveChanges();
                                                
                        Producto.FechaModificacionPrecio = DateTime.Now;
                        context.Entry(Producto).State = EntityState.Modified;
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
        public async Task<ActionResult<ProductoDTO>> ActivarDesactivarProducto(int id, [FromBody] ProductoDTO ProductoModificacionDTO)
        {
            try
            {
                if (!ProductoExists(id)) { return NotFound(); }

                var Producto = mapper.Map<Producto>(ProductoModificacionDTO);
                var fecha = DateTime.Now;

                Producto.ProductoId = id;
                Producto.FechaUltimaModificacion = DateTime.Now;

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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>> DeleteProducto(int id)
        {
            try
            {
                if (!ProductoExists(id)) { return NotFound(); }

                context.Productos.Remove(new Producto() { ProductoId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool ProductoExists(int id)
        {
            return context.Productos.Any(x => x.ProductoId == id);
        }


        [HttpGet("ObtenerProductoByNoParte")]
        public async Task<ActionResult<ProductoDTO>> ObtenerProductoByNoParte([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Productos = await context.Productos.Include(x => x.Marca).Where(x => x.NoParte.Contains(filtrosProducto.NoParte)).ToListAsync();
                return mapper.Map<ProductoDTO>(Productos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("ObtenerProductoByFilter")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> ObtenerProductoByFilter([FromQuery] ProductoFilter filtrosProducto)
        {
            try
            {
                var Productos = await context.Productos.Include(x => x.Marca).Where(x =>
                    (x.NoParte.Contains(filtrosProducto.NoParte) && filtrosProducto.NoParte != null) ||
                    (x.Nombre.Contains(filtrosProducto.Nombre) && filtrosProducto.Nombre != null) ||
                    (x.Modelo.Contains(filtrosProducto.Modelo) && filtrosProducto.Modelo != null) ||
                    (x.MarcaId == filtrosProducto.MarcaId && filtrosProducto.MarcaId != 0)
                ).ToListAsync();
                return mapper.Map<List<ProductoDTO>>(Productos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetHistorialPrecioProducto/{productoId}")]
        public async Task<ActionResult<List<HistorialPreciosProductosDTO>>> GetHistorialPrecioProducto(int productoId)
        {
            try
            {
                var historial = await context.HistorialPreciosProductos.Where(x => x.ProductoId == productoId).OrderByDescending(x => x.FechaRegistro).ToListAsync();
                if (historial == null)
                    historial = new List<HistorialPreciosProductos>();

                return mapper.Map<List<HistorialPreciosProductosDTO>>(historial);
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
