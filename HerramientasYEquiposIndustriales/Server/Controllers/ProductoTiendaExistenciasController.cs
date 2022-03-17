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
    public class ProductoTiendaExistenciasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductoTiendaExistenciasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoTiendaExistenciasDTO>>> GetExistencias()
        {
            try
            {
                var existencias = await context.ProductoTiendaExistencias.Include(x => x.ProductoTienda).ToListAsync();
                return mapper.Map<List<ProductoTiendaExistenciasDTO>>(existencias);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de ProductosTiendas. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("GetExistenciasByFilter")]
        public async Task<ActionResult<IEnumerable<ExistenciasBusquedaDTO>>> GetExistenciasByFilter([FromQuery] ExistenciasTiendaFilter filtro)
        {
            try
            {
                //USADO, NUEVO
                var consultaNuevos = from te in context.ProductoTiendaExistencias
                                     where (!te.Usado) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty)
                                     group te by new { te.ProductoTiendaId } into r
                                     select new ContadorDTO()
                                     {
                                         Id = r.Key.ProductoTiendaId,
                                         Cantidad = r.Count()
                                     };

                var consultaUsados = from te in context.ProductoTiendaExistencias
                                     where (te.Usado) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty) //(filtro.TipoExistencia == 2 && te.Usado) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0)
                                     group te by new { te.ProductoTiendaId } into r
                                     select new ContadorDTO()
                                     {
                                         Id = r.Key.ProductoTiendaId,
                                         Cantidad = r.Count()
                                     };


                //PARA RENTA, PARA VENTA
                var consultaParaRenta = from te in context.ProductoTiendaExistencias
                                        where (te.ParaRenta) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty)
                                        group te by new { te.ProductoTiendaId } into r
                                        select new ContadorDTO()
                                        {
                                            Id = r.Key.ProductoTiendaId,
                                            Cantidad = r.Count()
                                        };

                var consultaParaVenta = from te in context.ProductoTiendaExistencias
                                        where (te.ParaVenta) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty)
                                        group te by new { te.ProductoTiendaId } into r
                                        select new ContadorDTO()
                                        {
                                            Id = r.Key.ProductoTiendaId,
                                            Cantidad = r.Count()
                                        };


                //RENTADO, DISPONIBLE
                var consultaRentados = from te in context.ProductoTiendaExistencias
                                       where (te.Rentado) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty)
                                       group te by new { te.ProductoTiendaId } into r
                                       select new ContadorDTO()
                                       {
                                           Id = r.Key.ProductoTiendaId,
                                           Cantidad = r.Count()
                                       };

                var consultaDisponibles = from te in context.ProductoTiendaExistencias
                                          where (!te.Rentado) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty)
                                          group te by new { te.ProductoTiendaId } into r
                                          select new ContadorDTO()
                                          {
                                              Id = r.Key.ProductoTiendaId,
                                              Cantidad = r.Count()
                                          };


                //VENDIDO, NO VENDIDO
                var consultaVendidos = from te in context.ProductoTiendaExistencias
                                       where (te.Vendido) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty)
                                       group te by new { te.ProductoTiendaId } into r
                                       select new ContadorDTO()
                                       {
                                           Id = r.Key.ProductoTiendaId,
                                           Cantidad = r.Count()
                                       };

                var consultaNoVendidos = from te in context.ProductoTiendaExistencias
                                         where (!te.Vendido) && (te.ProductoTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0 || filtro.Sku != string.Empty)
                                         group te by new { te.ProductoTiendaId } into r
                                         select new ContadorDTO()
                                         {
                                             Id = r.Key.ProductoTiendaId,
                                             Cantidad = r.Count()
                                         };

                //var consultaProd = from pt in context.ProductosTienda.Include(x => x.Marca)
                //                   where (pt.ProductosTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0) && (pt.Sku.Contains(filtro.Sku) || filtro.Sku == string.Empty)
                //                   join pro in context.ProductoTiendaExistencias on pt.ProductosTiendaId equals pro.ProductoTiendaId into cPro
                //                   from gPro in cPro.DefaultIfEmpty()


                var consultaFinal = from pt in context.ProductosTienda.Include(x => x.Marca)
                                    where (pt.ProductosTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0) && (pt.Sku.Contains(filtro.Sku) || (filtro.Sku ?? string.Empty) == string.Empty)

                                    //join pro in context.ProductosTienda on pt.ProductosTiendaId  equals pro.ProductosTiendaId into cPro
                                    //from gPro in cPro.DefaultIfEmpty()
                                    //where (gPro.Sku.Contains(filtro.Sku) || filtro.Sku == string.Empty)

                                    join cu in consultaUsados on pt.ProductosTiendaId equals cu.Id into cUs
                                    from gUs in cUs.DefaultIfEmpty()
                                    join cn in consultaNuevos on pt.ProductosTiendaId equals cn.Id into cNu
                                    from gNu in cNu.DefaultIfEmpty()
                                    join cpr in consultaParaRenta on pt.ProductosTiendaId equals cpr.Id into cPRe
                                    from gPRe in cPRe.DefaultIfEmpty()
                                    join cpv in consultaParaVenta on pt.ProductosTiendaId equals cpv.Id into cPVe
                                    from gPVe in cPVe.DefaultIfEmpty()
                                    join cr in consultaRentados on pt.ProductosTiendaId equals cr.Id into cRe
                                    from gRe in cRe.DefaultIfEmpty()
                                    join cd in consultaDisponibles on pt.ProductosTiendaId equals cd.Id into cDi
                                    from gDi in cDi.DefaultIfEmpty()
                                    join cv in consultaVendidos on pt.ProductosTiendaId equals cv.Id into cVe
                                    from gVe in cVe.DefaultIfEmpty()
                                    join cnv in consultaNoVendidos on pt.ProductosTiendaId equals cnv.Id into cNVe
                                    from gNVe in cNVe.DefaultIfEmpty()
                                    where //(pt.ProductosTiendaId == filtro.ProductoTiendaId || filtro.ProductoTiendaId == 0) &&
                                        ((filtro.TipoExistencia == 1 && gNu.Cantidad > 0) || (filtro.TipoExistencia == 2 && gUs.Cantidad > 0) || filtro.TipoExistencia == 0) &&
                                        ((filtro.TipoNegocio == 1 && gPRe.Cantidad > 0) || (filtro.TipoNegocio == 2 && gPVe.Cantidad > 0) || filtro.TipoNegocio == 0) &&
                                        ((filtro.TipoDispocicion == 1 && gRe.Cantidad > 0) || (filtro.TipoDispocicion == 2 && gDi.Cantidad > 0) || filtro.TipoDispocicion == 0) &&
                                        ((filtro.TipoVenta == 1 && gVe.Cantidad > 0) || (filtro.TipoVenta == 2 && gNVe.Cantidad > 0) || filtro.TipoVenta == 0)
                                    select new ExistenciasBusquedaDTO
                                    {
                                        ProductosTiendaId = pt.ProductosTiendaId,
                                        Sku = pt.Sku,
                                        Nombre = pt.Nombre,
                                        Marca = pt.Marca.Descripcion,
                                        Modelo = pt.Modelo,
                                        Usados = gUs.Cantidad,
                                        Nuevos = gNu.Cantidad,
                                        ParaRenta = gPRe.Cantidad,
                                        ParaVenta = gPVe.Cantidad,
                                        Rentados = gRe.Cantidad,
                                        Disponibles = gDi.Cantidad,
                                        Vendidos = gVe.Cantidad,
                                        NoVendidos = gNVe.Cantidad
                                    };

                return await consultaFinal.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener la información de los Productos. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPost]
        public ActionResult<ProductoTiendaExistenciasDTO> PostProductoTiendaExistencia(ProductoTiendaExistenciasDTO nuevaExistencia)
        {
            try
            {
                var existencia = mapper.Map<ProductoTiendaExistencias>(nuevaExistencia);
                existencia.FechaRegistro = DateTime.Now;
                ProductoTiendaExistenciasDTO dto;

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    context.ProductoTiendaExistencias.Add(existencia);
                    context.SaveChanges();

                    dto = mapper.Map<ProductoTiendaExistenciasDTO>(existencia);
                    scope.Complete();
                }
                return new CreatedAtRouteResult("ObtenerProducto", new { id = existencia.ProductoTiendaExistenciasId }, dto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al crear el Producto. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpPut("ActualizaExistencia")]
        public async Task<ActionResult<ProductoTiendaExistenciasDTO>> ActualizaExistencia([FromBody] ProductoTiendaExistenciasDTO existenciaActualizar)
        {
            try
            {
                var existencia = mapper.Map<ProductoTiendaExistencias>(existenciaActualizar);
                var fecha = DateTime.Now;

                existencia.FechaModificacion = DateTime.Now;

                context.Entry(existencia).State = EntityState.Modified;
                context.Entry(existencia).Property(x => x.FechaRegistro).IsModified = false;
                context.Entry(existencia).Property(x => x.EmpleadoRegistro).IsModified = false;

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


        [HttpGet("GetExistenciasByProductoTienda/{productoTiendaId}")]
        public async Task<ActionResult<IEnumerable<ProductoTiendaExistenciasDTO>>> GetExistenciasByProductoTiendaAsync(int productoTiendaId)
        {
            try
            {
                var existencias = await context.ProductoTiendaExistencias.Where(x => x.ProductoTiendaId == productoTiendaId).ToListAsync();
                return mapper.Map<List<ProductoTiendaExistenciasDTO>>(existencias);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }


        [HttpGet("SiguienteFolioByProductoTienda/{productoTiendaId}")]
        public ActionResult<int> SiguienteFolioByProductoTienda(int productoTiendaId)
        {
            try
            {
                int ultimoNumeroProducto= 0;
                if (context.ProductoTiendaExistencias.Where(x=> x.ProductoTiendaId == productoTiendaId).Any())
                    ultimoNumeroProducto = context.ProductoTiendaExistencias.Where(x => x.ProductoTiendaId == productoTiendaId).Max(x => x.FolioProductoTienda);
                //return $"{DateTime.Now.Year}-{(ultimoNumeroEmpleado + 1):D4}";
                return ultimoNumeroProducto + 1;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{CommonConstant.MSG_ERROR_INICIO} " +
                    $"al obtener el listado de empleados. \n{CommonConstant.MSG_ERROR_FIN}");
            }
        }
    }
}
