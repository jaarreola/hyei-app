using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PuedeEditar",
                table: "Empleados",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CotizacionDetalles",
                columns: table => new
                {
                    CotizacionDetalleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CotizacionId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<float>(nullable: true),
                    CostoUnitario = table.Column<float>(nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    EmpleadoModificacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CotizacionDetalles", x => x.CotizacionDetalleId);
                    table.ForeignKey(
                        name: "FK_CotizacionDetalles_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cotizaciones",
                columns: table => new
                {
                    CotizacionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenTrabajoDetalleId = table.Column<int>(nullable: false),
                    Comentario = table.Column<string>(maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true),
                    EmpleadoModificacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizaciones", x => x.CotizacionId);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_OrdenTrabajoDetalle_OrdenTrabajoDetalleId",
                        column: x => x.OrdenTrabajoDetalleId,
                        principalTable: "OrdenTrabajoDetalle",
                        principalColumn: "OrdenTrabajoDetalleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstatusOTs",
                columns: table => new
                {
                    EstatusOTId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 100, nullable: true),
                    Posicion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusOTs", x => x.EstatusOTId);
                });

            migrationBuilder.CreateTable(
                name: "EstatusOTFlujos",
                columns: table => new
                {
                    EstatusOTFlujoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstatusOTId = table.Column<int>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(maxLength: 100, nullable: true),
                    EmpleadoCreacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusOTFlujos", x => x.EstatusOTFlujoId);
                    table.ForeignKey(
                        name: "FK_EstatusOTFlujos_EstatusOTs_EstatusOTId",
                        column: x => x.EstatusOTId,
                        principalTable: "EstatusOTs",
                        principalColumn: "EstatusOTId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionDetalles_ProductoId",
                table: "CotizacionDetalles",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_OrdenTrabajoDetalleId",
                table: "Cotizaciones",
                column: "OrdenTrabajoDetalleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstatusOTFlujos_EstatusOTId",
                table: "EstatusOTFlujos",
                column: "EstatusOTId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CotizacionDetalles");

            migrationBuilder.DropTable(
                name: "Cotizaciones");

            migrationBuilder.DropTable(
                name: "EstatusOTFlujos");

            migrationBuilder.DropTable(
                name: "EstatusOTs");

            migrationBuilder.DropColumn(
                name: "PuedeEditar",
                table: "Empleados");
        }
    }
}
