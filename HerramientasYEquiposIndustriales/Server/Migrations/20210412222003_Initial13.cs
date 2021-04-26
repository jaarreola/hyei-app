using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "OrdenTrabajoDetalle");

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "EstatusOTFlujos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionDetalles_CotizacionId",
                table: "CotizacionDetalles",
                column: "CotizacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CotizacionDetalles_Cotizaciones_CotizacionId",
                table: "CotizacionDetalles",
                column: "CotizacionId",
                principalTable: "Cotizaciones",
                principalColumn: "CotizacionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CotizacionDetalles_Cotizaciones_CotizacionId",
                table: "CotizacionDetalles");

            migrationBuilder.DropIndex(
                name: "IX_CotizacionDetalles_CotizacionId",
                table: "CotizacionDetalles");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "EstatusOTFlujos");

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "OrdenTrabajoDetalle",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
