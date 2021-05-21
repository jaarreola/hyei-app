﻿// <auto-generated />
using System;
using HerramientasYEquiposIndustriales.Server.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210508012700_Initial15")]
    partial class Initial15
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Cliente", b =>
                {
                    b.Property<int>("ClienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Correo")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<bool?>("EsFrecuente")
                        .HasColumnType("bit");

                    b.Property<bool?>("EsProblema")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("RFC")
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("ClienteId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Cotizacion", b =>
                {
                    b.Property<int>("CotizacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoModificacion")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrdenTrabajoDetalleId")
                        .HasColumnType("int");

                    b.HasKey("CotizacionId");

                    b.HasIndex("OrdenTrabajoDetalleId")
                        .IsUnique();

                    b.ToTable("Cotizaciones");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.CotizacionDetalle", b =>
                {
                    b.Property<int>("CotizacionDetalleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("Cantidad")
                        .HasColumnType("real");

                    b.Property<float?>("CostoUnitario")
                        .HasColumnType("real");

                    b.Property<int>("CotizacionId")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoModificacion")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.HasKey("CotizacionDetalleId");

                    b.HasIndex("CotizacionId");

                    b.HasIndex("ProductoId");

                    b.ToTable("CotizacionDetalles");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Empleado", b =>
                {
                    b.Property<int>("EmpleadoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Curp")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("MotivoBaja")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("Nss")
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("NumeroEmpleado")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("PuedeEditar")
                        .HasColumnType("bit");

                    b.Property<int>("PuestoId")
                        .HasColumnType("int");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("EmpleadoId");

                    b.HasIndex("NumeroEmpleado")
                        .IsUnique();

                    b.HasIndex("PuestoId");

                    b.ToTable("Empleados");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.EstatusOT", b =>
                {
                    b.Property<int>("EstatusOTId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Posicion")
                        .HasColumnType("int");

                    b.HasKey("EstatusOTId");

                    b.HasIndex("Descripcion")
                        .IsUnique()
                        .HasFilter("[Descripcion] IS NOT NULL");

                    b.ToTable("EstatusOTs");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.EstatusOTFlujo", b =>
                {
                    b.Property<int>("EstatusOTFlujoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(5000);

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int>("EstatusOTId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2")
                        .HasMaxLength(100);

                    b.Property<int>("OrdenTrabajoDetalleId")
                        .HasColumnType("int");

                    b.Property<bool?>("Terminado")
                        .HasColumnType("bit");

                    b.Property<string>("Ubicacion")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("EstatusOTFlujoId");

                    b.HasIndex("EstatusOTId");

                    b.HasIndex("OrdenTrabajoDetalleId");

                    b.ToTable("EstatusOTFlujos");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.FacturaMovimiento", b =>
                {
                    b.Property<int>("FacturaMovimientoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoModificacion")
                        .HasColumnType("int");

                    b.Property<string>("Factura")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.HasKey("FacturaMovimientoId");

                    b.HasIndex("Factura")
                        .IsUnique()
                        .HasFilter("[Factura] IS NOT NULL");

                    b.ToTable("FacturaMovimientos");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Marca", b =>
                {
                    b.Property<int>("MarcaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("MarcaId");

                    b.HasIndex("Descripcion")
                        .IsUnique();

                    b.ToTable("Marcas");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Movimiento", b =>
                {
                    b.Property<int>("MovimientoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal?>("Cantidad")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Comentario")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoModificacion")
                        .HasColumnType("int");

                    b.Property<bool>("EsEntrada")
                        .HasColumnType("bit");

                    b.Property<bool>("EsSalida")
                        .HasColumnType("bit");

                    b.Property<int?>("FacturaMovimientoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.HasKey("MovimientoId");

                    b.HasIndex("FacturaMovimientoId");

                    b.HasIndex("ProductoId");

                    b.ToTable("Movimientos");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.OrdenTrabajo", b =>
                {
                    b.Property<int>("OrdenTrabajoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClienteId")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoModificacion")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.HasKey("OrdenTrabajoId");

                    b.HasIndex("ClienteId");

                    b.ToTable("OrdenTrabajo");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.OrdenTrabajoDetalle", b =>
                {
                    b.Property<int>("OrdenTrabajoDetalleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentarios")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoModificacion")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaEntrega")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaFinaliacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("GarantiaFabrica")
                        .HasColumnType("bit");

                    b.Property<string>("GarantiaFabricaDetalle")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<bool>("GarantiaLocal")
                        .HasColumnType("bit");

                    b.Property<string>("GarantiaLocalDetalle")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Marca")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Modelo")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("NombreHerramienta")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("NumeroOrdenTrabajo")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("NumeroSerie")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("OrdenTrabajoId")
                        .HasColumnType("int");

                    b.Property<string>("TiempoGarantia")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<bool>("TieneCotizacion")
                        .HasColumnType("bit");

                    b.HasKey("OrdenTrabajoDetalleId");

                    b.HasIndex("OrdenTrabajoId");

                    b.ToTable("OrdenTrabajoDetalle");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Producto", b =>
                {
                    b.Property<int>("ProductoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("CantidadMinimaInventario")
                        .HasColumnType("real");

                    b.Property<float?>("CostoCompra")
                        .HasColumnType("real");

                    b.Property<float?>("CostoVenta")
                        .HasColumnType("real");

                    b.Property<int?>("EmpleadoActivo")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoBaja")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoCreacion")
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoModificacion")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaActivo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("MarcaId")
                        .HasColumnType("int");

                    b.Property<string>("Modelo")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("NoParte")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Ubicacion")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("ProductoId");

                    b.HasIndex("MarcaId");

                    b.HasIndex("NoParte")
                        .IsUnique();

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Puesto", b =>
                {
                    b.Property<int>("PuestoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("EsAdministrador")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaUltimaModificacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.HasKey("PuestoId");

                    b.ToTable("Puestos");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Cotizacion", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.OrdenTrabajoDetalle", "OrdenTrabajoDetalle")
                        .WithMany()
                        .HasForeignKey("OrdenTrabajoDetalleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.CotizacionDetalle", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.Cotizacion", "Cotizacion")
                        .WithMany()
                        .HasForeignKey("CotizacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Empleado", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.Puesto", "Puesto")
                        .WithMany()
                        .HasForeignKey("PuestoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.EstatusOTFlujo", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.EstatusOT", "EstatusOT")
                        .WithMany()
                        .HasForeignKey("EstatusOTId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.OrdenTrabajoDetalle", "OrdenTrabajoDetalle")
                        .WithMany()
                        .HasForeignKey("OrdenTrabajoDetalleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Movimiento", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.FacturaMovimiento", "FacturaMovimiento")
                        .WithMany()
                        .HasForeignKey("FacturaMovimientoId");

                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.OrdenTrabajo", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.OrdenTrabajoDetalle", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.OrdenTrabajo", "OrdenTrabajo")
                        .WithMany()
                        .HasForeignKey("OrdenTrabajoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HerramientasYEquiposIndustriales.Shared.Models.Producto", b =>
                {
                    b.HasOne("HerramientasYEquiposIndustriales.Shared.Models.Marca", "Marca")
                        .WithMany()
                        .HasForeignKey("MarcaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
