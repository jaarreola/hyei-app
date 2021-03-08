using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 40, nullable: false),
                    Apellido = table.Column<string>(maxLength: 40, nullable: true),
                    Telefono = table.Column<string>(maxLength: 10, nullable: true),
                    Correo = table.Column<string>(maxLength: 40, nullable: true),
                    Direccion = table.Column<string>(maxLength: 150, nullable: true),
                    RFC = table.Column<string>(maxLength: 13, nullable: true),
                    EsFrecuente = table.Column<bool>(nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
                });

            migrationBuilder.CreateTable(
                name: "FacturaMovimientos",
                columns: table => new
                {
                    FacturaMovimientoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Factura = table.Column<string>(maxLength: 20, nullable: true),
                    Descripcion = table.Column<string>(maxLength: 1000, nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    EmpleadoModificacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaMovimientos", x => x.FacturaMovimientoId);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    MarcaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.MarcaId);
                });

            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    PuestoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 80, nullable: false),
                    EsAdministrador = table.Column<bool>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.PuestoId);
                });

            migrationBuilder.CreateTable(
                name: "OrdenTrabajo",
                columns: table => new
                {
                    OrdenTrabajoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    EmpleadoModificacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenTrabajo", x => x.OrdenTrabajoId);
                    table.ForeignKey(
                        name: "FK_OrdenTrabajo_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoParte = table.Column<string>(maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    MarcaId = table.Column<int>(nullable: false),
                    Modelo = table.Column<string>(maxLength: 50, nullable: true),
                    CostoCompra = table.Column<float>(nullable: true),
                    CostoVenta = table.Column<float>(nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    EmpleadoModificacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ProductoId);
                    table.ForeignKey(
                        name: "FK_Productos_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "MarcaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    EmpleadoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroEmpleado = table.Column<string>(maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(maxLength: 80, nullable: true),
                    Direccion = table.Column<string>(maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(maxLength: 10, nullable: true),
                    PuestoId = table.Column<int>(nullable: false),
                    Activo = table.Column<bool>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    FechaBaja = table.Column<DateTime>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    MotivoBaja = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.EmpleadoId);
                    table.ForeignKey(
                        name: "FK_Empleados_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "PuestoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdenTrabajoDetalle",
                columns: table => new
                {
                    OrdenTrabajoDetalleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenTrabajoId = table.Column<int>(nullable: false),
                    NumeroOrdenTrabajo = table.Column<string>(maxLength: 10, nullable: true),
                    NombreHerramienta = table.Column<string>(maxLength: 100, nullable: true),
                    Marca = table.Column<string>(maxLength: 100, nullable: true),
                    Modelo = table.Column<string>(maxLength: 100, nullable: true),
                    NumeroSerie = table.Column<string>(maxLength: 100, nullable: true),
                    GarantiaFabrica = table.Column<bool>(nullable: false),
                    GarantiaFabricaDetalle = table.Column<string>(maxLength: 500, nullable: true),
                    GarantiaLocal = table.Column<bool>(nullable: false),
                    GarantiaLocalDetalle = table.Column<string>(maxLength: 500, nullable: true),
                    TiempoGarantia = table.Column<string>(maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    EmpleadoModificacion = table.Column<int>(nullable: true),
                    FechaEntrega = table.Column<DateTime>(nullable: true),
                    FechaFinaliacion = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenTrabajoDetalle", x => x.OrdenTrabajoDetalleId);
                    table.ForeignKey(
                        name: "FK_OrdenTrabajoDetalle_OrdenTrabajo_OrdenTrabajoId",
                        column: x => x.OrdenTrabajoId,
                        principalTable: "OrdenTrabajo",
                        principalColumn: "OrdenTrabajoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    MovimientoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(nullable: false),
                    EsEntrada = table.Column<bool>(nullable: false),
                    EsSalida = table.Column<bool>(nullable: false),
                    Cantidad = table.Column<decimal>(nullable: true),
                    Comentario = table.Column<string>(maxLength: 1000, nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    EmpleadoModificacion = table.Column<int>(nullable: true),
                    FacturaMovimientoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.MovimientoId);
                    table.ForeignKey(
                        name: "FK_Movimientos_FacturaMovimientos_FacturaMovimientoId",
                        column: x => x.FacturaMovimientoId,
                        principalTable: "FacturaMovimientos",
                        principalColumn: "FacturaMovimientoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimientos_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_NumeroEmpleado",
                table: "Empleados",
                column: "NumeroEmpleado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_PuestoId",
                table: "Empleados",
                column: "PuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_FacturaMovimientoId",
                table: "Movimientos",
                column: "FacturaMovimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_ProductoId",
                table: "Movimientos",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenTrabajo_ClienteId",
                table: "OrdenTrabajo",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenTrabajoDetalle_OrdenTrabajoId",
                table: "OrdenTrabajoDetalle",
                column: "OrdenTrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_MarcaId",
                table: "Productos",
                column: "MarcaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "OrdenTrabajoDetalle");

            migrationBuilder.DropTable(
                name: "Puestos");

            migrationBuilder.DropTable(
                name: "FacturaMovimientos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "OrdenTrabajo");

            migrationBuilder.DropTable(
                name: "Marcas");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
