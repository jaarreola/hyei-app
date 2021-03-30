using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrdenTrabajoDetalleId",
                table: "EstatusOTFlujos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Marcas_Descripcion",
                table: "Marcas",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstatusOTs_Descripcion",
                table: "EstatusOTs",
                column: "Descripcion",
                unique: true,
                filter: "[Descripcion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EstatusOTFlujos_OrdenTrabajoDetalleId",
                table: "EstatusOTFlujos",
                column: "OrdenTrabajoDetalleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstatusOTFlujos_OrdenTrabajoDetalle_OrdenTrabajoDetalleId",
                table: "EstatusOTFlujos",
                column: "OrdenTrabajoDetalleId",
                principalTable: "OrdenTrabajoDetalle",
                principalColumn: "OrdenTrabajoDetalleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstatusOTFlujos_OrdenTrabajoDetalle_OrdenTrabajoDetalleId",
                table: "EstatusOTFlujos");

            migrationBuilder.DropIndex(
                name: "IX_Marcas_Descripcion",
                table: "Marcas");

            migrationBuilder.DropIndex(
                name: "IX_EstatusOTs_Descripcion",
                table: "EstatusOTs");

            migrationBuilder.DropIndex(
                name: "IX_EstatusOTFlujos_OrdenTrabajoDetalleId",
                table: "EstatusOTFlujos");

            migrationBuilder.DropColumn(
                name: "OrdenTrabajoDetalleId",
                table: "EstatusOTFlujos");
        }
    }
}
