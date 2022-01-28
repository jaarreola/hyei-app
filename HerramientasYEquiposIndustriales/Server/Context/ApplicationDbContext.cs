﻿using HerramientasYEquiposIndustriales.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerramientasYEquiposIndustriales.Server.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Empleado>()
                .HasIndex(e => e.NumeroEmpleado)
                .IsUnique();

            builder.Entity<Producto>()
                .HasIndex(e => e.NoParte)
                .IsUnique();

            builder.Entity<FacturaMovimiento>()
                .HasIndex(e => e.Factura)
                .IsUnique();

            builder.Entity<Cotizacion>()
                .HasIndex(e => e.OrdenTrabajoDetalleId)
                .IsUnique();

            builder.Entity<EstatusOT>()
                .HasIndex(e => e.Descripcion)
                .IsUnique();

            builder.Entity<Marca>()
                .HasIndex(e => e.Descripcion)
                .IsUnique();


            builder.Entity<Configuraciones>(e =>
                {
                    e.HasNoKey();
                });

            //builder.Entity<OrdenTrabajoDetalle>(e =>
            //    {
            //        e.HasNoKey();
            //    });
        }

        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<MarcaHerramienta> MarcaHerramientas { get; set; }

        public DbSet<OrdenTrabajo> OrdenTrabajo { get; set; }
        public DbSet<OrdenTrabajoDetalle> OrdenTrabajoDetalle { get; set; }

        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<FacturaMovimiento> FacturaMovimientos { get; set; }

        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<CotizacionDetalle> CotizacionDetalles { get; set; }

        public DbSet<EstatusOT> EstatusOTs { get; set; }
        public DbSet<EstatusOTFlujo> EstatusOTFlujos { get; set; }

        public DbSet<Configuraciones> Configuraciones { get; set; }

        public DbSet<HistorialPreciosProductos> HistorialPreciosProductos { get; set; }


        //RENTA DE EQUIPOS Y PRODUCTOS TIENDA
        public DbSet<ProductosTienda> ProductosTienda { get; set; }
        public DbSet<ProductoTiendaExistencias> ProductoTiendaExistencias { get; set; }
        public DbSet<Rentas> Rentas { get; set; }
        public DbSet<ContratosRentas> ContratosRentas { get; set; }
        public DbSet<Aval> Aval { get; set; }
        public DbSet<MarcasProductosTienda> MarcasProductosTienda { get; set; }
    }
}
