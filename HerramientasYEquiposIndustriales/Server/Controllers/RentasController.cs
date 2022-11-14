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
    public class RentasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RentasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpPost("GuardaRentas")]
        public ActionResult<bool> GuardaRentas(List<object> datos)
        {
            try
            {
                Cliente cliente = new Cliente();
                Empleado empleado = new Empleado();
                List<Rentas> rentas = new List<Rentas>();

                if (datos.Count == 3)
                {
                    cliente = mapper.Map<Cliente>(JsonConvert.DeserializeObject<ClienteDTO>(datos[0].ToString()));
                    empleado = mapper.Map<Empleado>(JsonConvert.DeserializeObject<EmpleadoDTO>(datos[1].ToString()));
                    foreach (RentasDTO renta in (JsonConvert.DeserializeObject<List<RentasDTO>>(datos[2].ToString())))
                    {
                        //se agregan las rentas
                        rentas.Add(mapper.Map<Rentas>(renta));
                    }
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    foreach (var renta in rentas)
                    {
                        renta.ClienteId = cliente.ClienteId;
                        renta.EmpleadoCreacion = empleado.EmpleadoId;
                        renta.FechaRegistro = DateTime.Now;
                        context.Rentas.Add(renta);
                        context.SaveChanges();

                        //por cada renta hay que afectar la existencia del producto
                        var productoExistencia = context.ProductoTiendaExistencias.Where(x => x.ProductoTiendaExistenciasId == renta.ProductoTiendaExistenciasId).FirstOrDefault();
                        productoExistencia.FechaModificacion = DateTime.Now;
                        productoExistencia.EmpleadoModificacion = empleado.EmpleadoId;
                        productoExistencia.Rentado = true;

                        context.Entry(productoExistencia).State = EntityState.Modified;
                        context.Entry(productoExistencia).Property(x => x.FechaRegistro).IsModified = false;
                        context.Entry(productoExistencia).Property(x => x.EmpleadoRegistro).IsModified = false;
                        context.SaveChanges();
                    }
                    scope.Complete();
                }
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al generar la renta de la herramienta. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpPost("GuardaRenta")]
        public ActionResult<RentasDTO> GuardaRenta(List<object> datos)
        {
            try
            {
                Cliente cliente = new Cliente();
                Empleado empleado = new Empleado();
                Rentas renta = new Rentas();

                if (datos.Count == 3)
                {
                    cliente = mapper.Map<Cliente>(JsonConvert.DeserializeObject<ClienteDTO>(datos[0].ToString()));
                    empleado = mapper.Map<Empleado>(JsonConvert.DeserializeObject<EmpleadoDTO>(datos[1].ToString()));
                    renta = mapper.Map<Rentas>(JsonConvert.DeserializeObject<RentasDTO>(datos[2].ToString()));
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    if(renta.RentasId == 0)
                    {
                        renta.ClienteId = cliente.ClienteId;
                        renta.EmpleadoCreacion = empleado.EmpleadoId;
                        renta.FechaRegistro = DateTime.Now;
                        context.Rentas.Add(renta);
                        context.SaveChanges();
                    }
                    else
                    {
                        renta.ClienteId = cliente.ClienteId;
                        renta.EmpleadoCreacion = empleado.EmpleadoId;
                        context.Entry(renta).State = EntityState.Modified;
                        context.Entry(renta).Property(x => x.FechaRegistro).IsModified = false;
                        context.SaveChanges();
                    }
                    scope.Complete();
                }
                return mapper.Map<RentasDTO>(renta);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al generar la renta de la herramienta. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost("CompletaRenta")]
        public ActionResult<int> CompletaRenta(List<object> datos)
        {
            try
            {
                Cliente cliente = new Cliente();
                Empleado empleado = new Empleado();
                List<Rentas> rentas = new List<Rentas>();
                int sigNumeroRenta = GetSiguienteNumeroRenta();

                if (datos.Count == 3)
                {
                    cliente = mapper.Map<Cliente>(JsonConvert.DeserializeObject<ClienteDTO>(datos[0].ToString()));
                    empleado = mapper.Map<Empleado>(JsonConvert.DeserializeObject<EmpleadoDTO>(datos[1].ToString()));
                    foreach (RentasDTO renta in (JsonConvert.DeserializeObject<List<RentasDTO>>(datos[2].ToString())))
                    {
                        //se agregan las rentas
                        rentas.Add(mapper.Map<Rentas>(renta));
                    }
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    foreach (var renta in rentas)
                    {
                        var rentaActualiza = context.Rentas.Where(x => x.RentasId == renta.RentasId).FirstOrDefault();
                        rentaActualiza.ClienteId = cliente.ClienteId;
                        rentaActualiza.EmpleadoCreacion = empleado.EmpleadoId;
                        rentaActualiza.NoRenta = sigNumeroRenta;
                        rentaActualiza.Generada = true;

                        context.Entry(rentaActualiza).State = EntityState.Modified;
                        context.Entry(rentaActualiza).Property(x => x.FechaRegistro).IsModified = false;
                        context.SaveChanges();

                        //por cada renta hay que afectar la existencia del producto
                        var productoExistencia = context.ProductoTiendaExistencias.Where(x => x.ProductoTiendaExistenciasId == renta.ProductoTiendaExistenciasId).FirstOrDefault();
                        productoExistencia.FechaModificacion = DateTime.Now;
                        productoExistencia.EmpleadoModificacion = empleado.EmpleadoId;
                        productoExistencia.Rentado = true;

                        context.Entry(productoExistencia).State = EntityState.Modified;
                        context.Entry(productoExistencia).Property(x => x.FechaRegistro).IsModified = false;
                        context.Entry(productoExistencia).Property(x => x.EmpleadoRegistro).IsModified = false;
                        context.SaveChanges();
                    }
                    scope.Complete();
                }
                return sigNumeroRenta;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al generar la renta de la herramienta. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpGet("GetRentasByFilter")]
        public async Task<ActionResult<IEnumerable<RentasDetalleDTO>>> GetRentasByFilter([FromQuery] BusquedaRentaFilter filtro)
        {
            try
            {
                var rentas = from te in context.ProductoTiendaExistencias
                             join pt in context.ProductosTienda on te.ProductoTiendaId equals pt.ProductosTiendaId //into cUs
                             join r in context.Rentas on te.ProductoTiendaExistenciasId equals r.ProductoTiendaExistenciasId
                             join c in context.Clientes on r.ClienteId equals c.ClienteId
                             where
                                //(te.Rentado) &&
                                ((filtro.EstatusRenta > 0 && ((filtro.EstatusRenta == 1 && (r.FechaFinRenta >= DateTime.Today || r.FechaFinRenta < DateTime.Today) && r.FechaEntrega == null) || (filtro.EstatusRenta == 2 && r.FechaFinRenta < DateTime.Today && r.FechaEntrega == null) || (filtro.EstatusRenta == 3 && r.FechaEntrega != null))) || filtro.EstatusRenta == 0) &&

                                //((r.RentasId == filtro.RentasId || filtro.RentasId == 0) && (r.FechaInicioRenta.Value.Date == filtro.FechaInicioRenta || filtro.FechaInicioRenta == null) && (r.FechaFinRenta.Value.Date <= filtro.FechaFinRenta || filtro.FechaFinRenta == null)) &&
                                ((r.NoRenta == filtro.RentasId || filtro.RentasId == 0) && (r.FechaInicioRenta.Value.Date == filtro.FechaInicioRenta || filtro.FechaInicioRenta == null) && (r.FechaFinRenta.Value.Date <= filtro.FechaFinRenta || filtro.FechaFinRenta == null)) &&
                                ((c.ClienteId == filtro.ClienteId || filtro.ClienteId == 0) && (c.Nombre.Contains(filtro.NombreCliente) || filtro.NombreCliente == null) && (c.Apellido.Contains(filtro.ApellidoCliente) || filtro.ApellidoCliente == null) && (c.RFC.Contains(filtro.RFC) || filtro.RFC == null)) &&
                                ((pt.Sku.Contains(filtro.Sku) || filtro.Sku == null) && (pt.ProductosTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0))
                             select new RentasDetalleDTO()
                             {
                                 ProductosTiendaId = pt.ProductosTiendaId,
                                 Sku = pt.Sku,
                                 Herramienta = pt.Nombre,
                                 RentaId = r.RentasId,
                                 ProductoTiendaExistenciasId = te.ProductoTiendaExistenciasId,
                                 Rentado = te.Rentado,
                                 FolioProductoTienda = te.FolioProductoTienda,
                                 FechaInicioRenta = r.FechaInicioRenta,
                                 FechaFinRenta = r.FechaFinRenta,
                                 FechaEntregado = r.FechaEntrega,
                                 TotalRenta = r.TotalRenta,
                                 TotalConRecargo = r.TotalConRecargo,
                                 ClienteId = c.ClienteId,
                                 Nombre = c.Nombre,
                                 Apellido = c.Apellido,
                                 Direccion = c.Direccion,
                                 Telefono = c.Telefono,
                                 HorasSalida = r.HorasSalida,
                                 HorasEntrega = r.HorasEntrega,
                                 Comentarios = r.Comentarios,
                                 NoRenta = r.NoRenta
                             };

                return await rentas.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpGet("GetRentaByRentaId/{rentaId}")]
        public async Task<ActionResult<RentasDTO>> GetRentaByRentaId(int rentaId)
        {
            try
            {
                var renta = await context.Rentas.FirstOrDefaultAsync(x => x.RentasId == rentaId);

                if (renta == null) { return NotFound(); }

                return mapper.Map<RentasDTO>(renta);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpPost("RecibirRentaByRenta")]
        public ActionResult<bool> RecibirRentaByRenta(RentasDTO rentaDto)
        //public async Task<ActionResult<bool>> RecibirRentaByRenta(RentasDTO rentaDto)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    if (rentaDto.RentasId > 0)
                    {
                        var renta = mapper.Map<Rentas>(rentaDto);
                        renta.FechaEntrega = DateTime.Now;
                        context.Entry(renta).State = EntityState.Modified;
                        context.Entry(renta).Property(x => x.FechaRegistro).IsModified = false;
                        context.Entry(renta).Property(x => x.EmpleadoCreacion).IsModified = false;
                        context.SaveChanges();

                        //ponemos disponible de nueva cuenta el producto que se recibio y actualizamos su total de horas de uso
                        var productoExistencia = context.ProductoTiendaExistencias.Where(x => x.ProductoTiendaExistenciasId == renta.ProductoTiendaExistenciasId).FirstOrDefault();
                        productoExistencia.FechaModificacion = DateTime.Now;
                        productoExistencia.EmpleadoModificacion = renta.EmpleadoModificacion;
                        productoExistencia.Rentado = false;
                        productoExistencia.TotalHorasRentado = (productoExistencia.TotalHorasRentado ?? 0) + renta.TotalHorasRenta;

                        context.Entry(productoExistencia).State = EntityState.Modified;
                        context.Entry(productoExistencia).Property(x => x.FechaRegistro).IsModified = false;
                        context.Entry(productoExistencia).Property(x => x.EmpleadoRegistro).IsModified = false;
                        context.SaveChanges();
                    }
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al recibir el Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }



        [HttpGet("GetMasRentadosByFilter")]
        public async Task<ActionResult<IEnumerable<HistorialProductosRentadosDTO>>> GetMasRentadosByFilter([FromQuery] BusquedaRentaFilter filtro)
        {
            try
            {
                var cantidadRentados = from pt in context.ProductosTienda
                                       join mp in context.MarcasProductosTienda on pt.MarcasProductosTiendaId equals mp.MarcasProductosTiendaId
                                       join te in context.ProductoTiendaExistencias on pt.ProductosTiendaId equals te.ProductoTiendaId
                                       join r in context.Rentas on te.ProductoTiendaExistenciasId equals r.ProductoTiendaExistenciasId
                                       where
                                       (pt.Sku == filtro.Sku || filtro.Sku == null) &&
                                       (pt.ProductosTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0)
                                       group new { pt, mp, r } by new { pt.ProductosTiendaId, pt.Sku, pt.Nombre, mp.Descripcion, pt.Modelo } into r
                                       select new HistorialProductosRentadosDTO()
                                       {
                                           ProdcutoTiendaId = r.Key.ProductosTiendaId,
                                           Sku = r.Key.Sku,
                                           NombreHerramienta = r.Key.Nombre,
                                           Marca = r.Key.Descripcion,
                                           Modelo = r.Key.Modelo,
                                           VecesRentado = r.Sum(x => 1) //r.Count(x => x.r.RentasId)
                                       };

                return await cantidadRentados.OrderByDescending(x => x.VecesRentado).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetHistorialRentasByProducto/{prodcutoTiendaId}")]
        public async Task<ActionResult<IEnumerable<HistorialProductosRentadosDTO>>> GetHistorialRentasByProducto(int prodcutoTiendaId)
        {
            try
            {
                var cantidadRentados = from pt in context.ProductosTienda
                                       join mp in context.MarcasProductosTienda on pt.MarcasProductosTiendaId equals mp.MarcasProductosTiendaId
                                       join te in context.ProductoTiendaExistencias on pt.ProductosTiendaId equals te.ProductoTiendaId
                                       join r in context.Rentas on te.ProductoTiendaExistenciasId equals r.ProductoTiendaExistenciasId
                                       where
                                        pt.ProductosTiendaId == prodcutoTiendaId
                                       select new HistorialProductosRentadosDTO()
                                       {
                                           ProdcutoTiendaId = pt.ProductosTiendaId,
                                           Sku = pt.Sku,
                                           NombreHerramienta = pt.Nombre,
                                           Marca = mp.Descripcion,
                                           Modelo = pt.Modelo,
                                           FolioProductoTienda = te.FolioProductoTienda,
                                           RentasId = r.RentasId,
                                           FechaInicioRenta = r.FechaInicioRenta,
                                           FechaFinRenta = r.FechaFinRenta,
                                           FechaEntrega = r.FechaEntrega
                                       };

                return await cantidadRentados.OrderByDescending(x => x.FechaFinRenta).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpDelete("DeleteRenta/{id}")]
        public async Task<ActionResult> DeleteRenta(int id)
        {
            try
            {
                if (!RentaExists(id)) { return NotFound(); }
                context.Rentas.Remove(new Rentas() { RentasId = id });
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al eliminar la renta. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }

        private bool RentaExists(int id)
        {
            return context.Rentas.Any(x => x.RentasId == id);
        }

        private int GetSiguienteNumeroRenta()
        {
            return context.Rentas.Max(x => x.NoRenta) + 1;
        }
    }
}
