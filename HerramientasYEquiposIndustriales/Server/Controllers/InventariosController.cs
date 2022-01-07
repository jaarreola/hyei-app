using AutoMapper;
using HerramientasYEquiposIndustriales.Server.Constants;
using HerramientasYEquiposIndustriales.Server.Context;
using HerramientasYEquiposIndustriales.Shared.DTOs;
using HerramientasYEquiposIndustriales.Shared.Filters;
using HerramientasYEquiposIndustriales.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HerramientasYEquiposIndustriales.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public InventariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoDTO>>> GetInventarios()
        {
            try
            {
                var Inventarios = await context.Movimientos.ToListAsync();
                return mapper.Map<List<MovimientoDTO>>(Inventarios);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de Movimientos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpGet("{id}", Name = "ObtenerMovimiento")]
        public async Task<ActionResult<MovimientoDTO>> GetMovimiento(int id)
        {
            try
            {
                var Movimiento = await context.Movimientos.FirstOrDefaultAsync(x => x.MovimientoId == id);

                if (Movimiento == null) { return NotFound(); }

                var dto = mapper.Map<MovimientoDTO>(Movimiento);

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


        [HttpGet("GetFactura/{factura}")]
        public async Task<ActionResult<FacturaMovimientoDTO>> GetFactura(String factura)
        {
            try
            {
                var facturaMovimiento = await context.FacturaMovimientos.FirstOrDefaultAsync(x => x.Factura == factura);

                if (facturaMovimiento == null) { return NotFound(); }

                var dto = mapper.Map<FacturaMovimientoDTO>(facturaMovimiento);

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


        [HttpPost("ObtenerRegistrosPorFactura")]
        public async Task<ActionResult<List<MovimientoDTO>>> GetRegistrosPorFactura([FromBody] int id)
        {
            try
            {
                var registros = await context.Movimientos.Include(x => x.FacturaMovimiento).Include(x => x.Producto).Where(x => x.FacturaMovimientoId == id).ToListAsync();

                if (registros == null) { return NotFound(); }

                return mapper.Map<List<MovimientoDTO>>(registros);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost("ObtenerRegistrosPorNumeroFactura")]
        public async Task<ActionResult<List<MovimientoDTO>>> ObtenerRegistrosPorNumeroFactura([FromBody] String numeroFactura)
        {
            try
            {
                var registros = await context.Movimientos.Include(x => x.FacturaMovimiento).Include(x => x.Producto).Where(x => x.FacturaMovimiento.Factura == numeroFactura).ToListAsync();

                if (registros == null) { return NotFound(); }

                return mapper.Map<List<MovimientoDTO>>(registros);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información del Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpPost]
        public async Task<ActionResult<MovimientoDTO>> PostMovimiento([FromBody] MovimientoDTO MovimientoCreacionDTO)
        {
            try
            {
                var Movimiento = mapper.Map<Movimiento>(MovimientoCreacionDTO);
                Movimiento.FechaRegistro = DateTime.Now;

                if (Movimiento.EsEntrada && Movimiento.EsSalida)
                    Movimiento.EsSalida = false;
                if (!Movimiento.EsEntrada && !Movimiento.EsSalida)
                    Movimiento.EsEntrada = true;

                context.Movimientos.Add(Movimiento);

                await context.SaveChangesAsync();

                var dto = mapper.Map<MovimientoDTO>(Movimiento);

                return new CreatedAtRouteResult("ObtenerMovimiento", new { id = Movimiento.MovimientoId }, dto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovimientoDTO>> PutMovimiento(int id, [FromBody] MovimientoDTO MovimientoModificacionDTO)
        {
            try
            {
                if (!MovimientoExists(id)) { return NotFound(); }

                var Movimiento = mapper.Map<Movimiento>(MovimientoModificacionDTO);

                Movimiento.MovimientoId = id;
                Movimiento.FechaUltimaModificacion = DateTime.Now;

                if (Movimiento.EsEntrada && Movimiento.EsSalida)
                    Movimiento.EsSalida = false;
                if (!Movimiento.EsEntrada && !Movimiento.EsSalida)
                    Movimiento.EsEntrada = true;

                context.Entry(Movimiento).State = EntityState.Modified;
                context.Entry(Movimiento).Property(x => x.FechaRegistro).IsModified = false;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al actualizar la información del Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovimiento(int id)
        {
            try
            {
                if (!MovimientoExists(id)) { return NotFound(); }

                context.Movimientos.Remove(new Movimiento() { MovimientoId = id });

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar el Movimiento. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool MovimientoExists(int id)
        {
            return context.Movimientos.Any(x => x.MovimientoId == id);
        }


        [HttpGet]
        [Route("/api/[Controller]/ObtenerMovimientosFilter")]
        public async Task<ActionResult<IEnumerable<MovimientoDTO>>> ObtenerMovimientosFilter([FromQuery] MovimientoFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var Productos = await context.Movimientos.Include(x => x.FacturaMovimiento).Include(x => x.Producto).Where(x =>
                    ((x.EsEntrada == true && filtro.TipoEntrada == 1) || (x.EsSalida == true && filtro.TipoEntrada == -1) || filtro.TipoEntrada == 0) &&
                    (x.FacturaMovimiento.Factura.Contains(filtro.Factura) || filtro.Factura == null || filtro.Factura == String.Empty) &&
                    (x.Producto.NoParte.Contains(filtro.NoParte) || filtro.NoParte == null || filtro.NoParte == String.Empty) &&
                    (x.Producto.Nombre.Contains(filtro.NombreParte) || filtro.NombreParte == null || filtro.NombreParte == String.Empty) &&
                    ((x.FechaRegistro.Value.Date >= filtro.FechaInicio && x.FechaRegistro.Value.Date < filtro.FechaFin) || filtro.FechaInicio == null || filtro.FechaFin == null)
                ).ToListAsync();
                return mapper.Map<List<MovimientoDTO>>(Productos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet]
        [Route("/api/[Controller]/BuscaFacturas")]
        public async Task<ActionResult<IEnumerable<FacturaMovimientoDTO>>> BuscaFacturas([FromQuery] FacturaFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                Boolean buscar = false;

                if (filtro.FechaFin != null)
                {
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                    buscar = true;
                }
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                {
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                    buscar = true;
                }
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var Facturas = await context.FacturaMovimientos.Where(x =>
                    (x.Factura.Contains(filtro.Factura) && filtro.Factura != null && filtro.Factura != String.Empty) ||
                    (x.Descripcion.Contains(filtro.Descripcion) && filtro.Descripcion != null && filtro.Descripcion != String.Empty) ||
                    ((x.FechaRegistro.Value.Date >= filtro.FechaInicio && x.FechaRegistro.Value.Date < filtro.FechaFin) && filtro.FechaInicio != null && filtro.FechaFin != null && buscar)
                ).ToListAsync();
                return mapper.Map<List<FacturaMovimientoDTO>>(Facturas);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de las Facturas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost("SaveFactura")]
        public ActionResult<bool> PostSaveFactura(List<object> datos)
        {
            try
            {
                FacturaMovimiento factura = new FacturaMovimiento();
                List<Movimiento> movimientos = new List<Movimiento>();
                Movimiento movimiento;

                if (datos.Count == 2)
                {
                    DateTime fecha = DateTime.Now;
                    factura = mapper.Map<FacturaMovimiento>(JsonConvert.DeserializeObject<FacturaMovimientoDTO>(datos[0].ToString()));
                    if (factura.FacturaMovimientoId == 0)
                        factura.FechaRegistro = fecha;
                    else
                        factura.FechaUltimaModificacion = fecha;
                    foreach (Movimiento m in (JsonConvert.DeserializeObject<List<Movimiento>>(datos[1].ToString())))
                    {
                        movimiento = mapper.Map<Movimiento>(m);
                        movimiento.FechaRegistro = fecha;
                        movimientos.Add(movimiento);
                    }

                    using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        if (factura.FacturaMovimientoId == 0)
                        {
                            context.FacturaMovimientos.Add(factura);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.Entry(factura).State = EntityState.Modified;
                            context.Entry(factura).Property(x => x.FechaRegistro).IsModified = false;
                            context.SaveChanges();
                        }

                        var idFacturaIdCreacion = context.FacturaMovimientos.FirstOrDefaultAsync(x => x.Factura == factura.Factura).Result.FacturaMovimientoId;
                        foreach (var m in movimientos)
                        {
                            if (m.MovimientoId == 0)
                            {
                                m.FacturaMovimientoId = idFacturaIdCreacion;
                                m.Producto = null;

                                context.Movimientos.Add(m);
                                context.SaveChanges();
                            }
                        }
                        scope.Complete();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear la Factura. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetCostoInventarioByFilter")]
        public async Task<ActionResult<IEnumerable<CostoInventarioDTO>>> GetCostoInventarioByFilter([FromQuery] CostosInventarioFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var consulta = from p in context.Productos
                               join m in context.Movimientos on p.ProductoId equals m.ProductoId into m1
                               from m2 in m1.DefaultIfEmpty()
                               where
                                   ((m2.FechaRegistro.Value.Date >= filtro.FechaInicio && m2.FechaRegistro.Value.Date < filtro.FechaFin) || (filtro.FechaInicio == null && filtro.FechaFin == null)) &&
                                   (p.NoParte.Contains(filtro.NoParte) || (filtro.NoParte == null)) &&
                                   (p.Nombre.Contains(filtro.NombreParte) || (filtro.NombreParte == null))
                               group new { m2, p } by new { p.ProductoId, p.NoParte, p.Nombre, p.CostoCompra, p.CostoVenta } into r
                               orderby r.Key.NoParte ascending
                               select new CostoInventarioDTO()
                               {
                                   ProductoId = r.Key.ProductoId,
                                   NoParte = r.Key.NoParte,
                                   Nombre = r.Key.Nombre,
                                   Entradas = r.Sum(x => (x.m2.EsEntrada ? x.m2.Cantidad : 0)),
                                   Salidas = r.Sum(x => (x.m2.EsSalida ? x.m2.Cantidad : 0)),
                                   CostoCompra = r.Key.CostoCompra != null ? (decimal)r.Key.CostoCompra : 0,
                                   CostoVenta = r.Key.CostoVenta != null ? (decimal)r.Key.CostoVenta : 0
                               };

                var consultaR = from c in consulta
                                select new CostoInventarioDTO()
                                {
                                    ProductoId = c.ProductoId,
                                    NoParte = c.NoParte,
                                    Nombre = c.Nombre,
                                    Entradas = c.Entradas,
                                    Salidas = c.Salidas,
                                    Existencias = (decimal)(c.Entradas - c.Salidas),
                                    CostoCompra = c.CostoCompra,
                                    CostoVenta = c.CostoVenta,
                                    TotalCostoCompra = (decimal)(c.Entradas - c.Salidas) * c.CostoCompra,
                                    TotalCostoVenta = (decimal)(c.Entradas - c.Salidas) * c.CostoVenta
                                };


                if (filtro.EnExistencia)
                    return await consultaR.Where(x => (x.Entradas > x.Salidas)).ToListAsync();
                else if (filtro.SinExistencia)
                    return await consultaR.Where(x => (x.Entradas <= x.Salidas)).ToListAsync();
                else
                    return await consultaR.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetProductoMovimientosByFilter")]
        public async Task<ActionResult<IEnumerable<MovimientoDTO>>> GetProductoMovimientosByFilter([FromQuery] CostosInventarioFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var movimientos = await context.Movimientos.Include(x => x.FacturaMovimiento).Include(x => x.Producto).Where(x =>
                    (x.Producto.NoParte == filtro.NoParte || filtro.NoParte == null || filtro.NoParte == String.Empty) &&
                    ((x.FechaRegistro.Value.Date >= filtro.FechaInicio && x.FechaRegistro.Value.Date < filtro.FechaFin) || filtro.FechaInicio == null || filtro.FechaFin == null)
                ).ToListAsync();
                return mapper.Map<List<MovimientoDTO>>(movimientos);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetCantidadByProductoId/{productoId}")]
        public async Task<ActionResult<decimal>> GetCantidadByProductoId(int productoId)
        {
            try
            {
                var consulta = from m in context.Movimientos
                               where (m.ProductoId == productoId)
                               group m by new { m.ProductoId } into r
                               select new MovimientoDTO()
                               {
                                   ProductoId = r.Key.ProductoId,
                                   Cantidad = r.Sum(x => (x.EsEntrada ? x.Cantidad : (-1 * x.Cantidad)))
                               };

                var registro = await consulta.FirstOrDefaultAsync(); //.Where(x => (x.Entradas > x.Salidas)).ToListAsync();
                if (registro == null) { return 0; }
                return registro.Cantidad;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetFaltantesByFilter")]
        public async Task<ActionResult<IEnumerable<CostoInventarioDTO>>> GetFaltantesByFilter([FromQuery] CostosInventarioFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var consulta = from p in context.Productos
                               join m in context.Movimientos on p.ProductoId equals m.ProductoId into m1
                               from m2 in m1.DefaultIfEmpty()
                               where
                                   ((m2.FechaRegistro.Value.Date >= filtro.FechaInicio && m2.FechaRegistro.Value.Date < filtro.FechaFin) || (filtro.FechaInicio == null && filtro.FechaFin == null)) &&
                                   (p.NoParte.Contains(filtro.NoParte) || (filtro.NoParte == null)) &&
                                   (p.Nombre.Contains(filtro.NombreParte) || (filtro.NombreParte == null))
                               group new { m2, p } by new { p.ProductoId, p.NoParte, p.Nombre, p.CantidadMinimaInventario, p.Ubicacion } into r
                               orderby r.Key.NoParte ascending
                               select new CostoInventarioDTO()
                               {
                                   ProductoId = r.Key.ProductoId,
                                   NoParte = r.Key.NoParte,
                                   Nombre = r.Key.Nombre,
                                   Entradas = r.Sum(x => (x.m2.EsEntrada ? x.m2.Cantidad : 0)),
                                   Salidas = r.Sum(x => (x.m2.EsSalida ? x.m2.Cantidad : 0)),
                                   CantidadMinimaInventario = r.Key.CantidadMinimaInventario != null ? (decimal)r.Key.CantidadMinimaInventario : 0,
                                   Ubicacion = r.Key.Ubicacion != null ? r.Key.Ubicacion : String.Empty
                               };

                var consultaR = from c in consulta
                                where (c.CantidadMinimaInventario >= (c.Entradas - c.Salidas))
                                select new CostoInventarioDTO()
                                {
                                    ProductoId = c.ProductoId,
                                    NoParte = c.NoParte,
                                    Nombre = c.Nombre,
                                    Entradas = c.Entradas,
                                    Salidas = c.Salidas,
                                    Existencias = (decimal)(c.Entradas - c.Salidas),
                                    CantidadMinimaInventario = c.CantidadMinimaInventario,
                                    Ubicacion = c.Ubicacion
                                };

                return await consultaR.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetEntradasByFilter")]
        public async Task<ActionResult<IEnumerable<CostoInventarioDTO>>> GetEntradasByFilter([FromQuery] CostosInventarioFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var consulta = from p in context.Productos
                               join m in context.Movimientos on p.ProductoId equals m.ProductoId
                               where
                                   ((m.FechaRegistro.Value.Date >= filtro.FechaInicio && m.FechaRegistro.Value.Date < filtro.FechaFin) || (filtro.FechaInicio == null && filtro.FechaFin == null)) &&
                                   (p.NoParte.Contains(filtro.NoParte) || (filtro.NoParte == null)) &&
                                   (p.Nombre.Contains(filtro.NombreParte) || (filtro.NombreParte == null)) &&
                                   m.EsEntrada
                               group new { m, p } by new { p.ProductoId, p.NoParte, p.Nombre, m.EsEntrada } into r
                               select new CostoInventarioDTO()
                               {
                                   ProductoId = r.Key.ProductoId,
                                   NoParte = r.Key.NoParte,
                                   Nombre = r.Key.Nombre,
                                   EsEntrada = r.Key.EsEntrada,
                                   Entradas = r.Sum(x => (x.m.EsEntrada ? x.m.Cantidad : 0))
                               };

                return await consulta.OrderByDescending(x => x.Entradas).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetMasCompradasByFilter")]
        public async Task<ActionResult<IEnumerable<CostoInventarioDTO>>> GetMasCompradasByFilter([FromQuery] CostosInventarioFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var consulta = from p in context.Productos
                               join m in context.Movimientos on p.ProductoId equals m.ProductoId
                               where
                                   ((m.FechaRegistro.Value.Date >= filtro.FechaInicio && m.FechaRegistro.Value.Date < filtro.FechaFin) || (filtro.FechaInicio == null && filtro.FechaFin == null)) &&
                                   (p.NoParte.Contains(filtro.NoParte) || (filtro.NoParte == null)) &&
                                   (p.Nombre.Contains(filtro.NombreParte) || (filtro.NombreParte == null)) &&
                                   m.EsEntrada
                               group new { m, p } by new { p.ProductoId, p.NoParte, p.Nombre, m.EsEntrada } into r
                               select new CostoInventarioDTO()
                               {
                                   ProductoId = r.Key.ProductoId,
                                   NoParte = r.Key.NoParte,
                                   Nombre = r.Key.Nombre,
                                   EsEntrada = r.Key.EsEntrada,
                                   Entradas = r.Sum(x => (x.m.EsEntrada ? 1 : 0))
                               };

                return await consulta.OrderByDescending(x => x.Entradas).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetSalidasByFilter")]
        public async Task<ActionResult<IEnumerable<CostoInventarioDTO>>> GetSalidasByFilter([FromQuery] CostosInventarioFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var consulta = from p in context.Productos
                               join m in context.Movimientos on p.ProductoId equals m.ProductoId
                               where
                                   ((m.FechaRegistro.Value.Date >= filtro.FechaInicio && m.FechaRegistro.Value.Date < filtro.FechaFin) || (filtro.FechaInicio == null && filtro.FechaFin == null)) &&
                                   (p.NoParte.Contains(filtro.NoParte) || (filtro.NoParte == null)) &&
                                   (p.Nombre.Contains(filtro.NombreParte) || (filtro.NombreParte == null)) &&
                                   m.EsSalida
                               group new { m, p } by new { p.ProductoId, p.NoParte, p.Nombre, m.EsSalida } into r
                               select new CostoInventarioDTO()
                               {
                                   ProductoId = r.Key.ProductoId,
                                   NoParte = r.Key.NoParte,
                                   Nombre = r.Key.Nombre,
                                   EsSalida = r.Key.EsSalida,
                                   Salidas = r.Sum(x => (x.m.EsSalida ? x.m.Cantidad : 0))
                               };

                return await consulta.OrderByDescending(x => x.Salidas).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetDetalleMovimientosByFilter")]
        public async Task<ActionResult<IEnumerable<CostoInventarioDTO>>> GetDetalleMovimientosByFilter([FromQuery] CostosInventarioFilter filtro)
        {
            try
            {
                int year = DateTime.Now.Year;
                if (filtro.FechaFin != null)
                    filtro.FechaFin = filtro.FechaFin.Value.Date.AddDays(1);
                else
                    filtro.FechaFin = new DateTime(year + 1, 1, 1).Date;

                if (filtro.FechaInicio != null)
                    filtro.FechaInicio = filtro.FechaInicio.Value.Date;
                else
                    filtro.FechaInicio = new DateTime(year, 1, 1).Date;

                var consulta = from p in context.Productos
                               join m in context.Movimientos on p.ProductoId equals m.ProductoId
                               join e in context.Empleados on (m.EmpleadoModificacion ?? m.EmpleadoCreacion) equals e.EmpleadoId
                               join f in context.FacturaMovimientos on m.FacturaMovimientoId equals f.FacturaMovimientoId into fa
                               from f1 in fa.DefaultIfEmpty()
                               where
                                   ((m.FechaRegistro.Value.Date >= filtro.FechaInicio && m.FechaRegistro.Value.Date < filtro.FechaFin) || (filtro.FechaInicio == null && filtro.FechaFin == null)) &&
                                   (p.NoParte == filtro.NoParte || filtro.NoParte == null) &&
                                   ((m.EsEntrada && filtro.EsEntrada) || (m.EsSalida && filtro.EsSalida))
                               select new CostoInventarioDTO()
                               {
                                   ProductoId = m.ProductoId,
                                   NoParte = p.NoParte,
                                   Nombre = p.Nombre,
                                   EsEntrada = m.EsEntrada,
                                   EsSalida = m.EsSalida,
                                   Cantidad = (decimal)m.Cantidad,
                                   FechaRegistro = m.FechaRegistro,
                                   NoEmpleado = e.NumeroEmpleado,
                                   Empleado = e.Nombre
                               };

                return await consulta.OrderByDescending(x => x.FechaRegistro).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

    }
}
