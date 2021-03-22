using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieneCoizacion",
                table: "OrdenTrabajoDetalle");

            migrationBuilder.AddColumn<bool>(
                name: "TieneCotizacion",
                table: "OrdenTrabajoDetalle",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieneCotizacion",
                table: "OrdenTrabajoDetalle");

            migrationBuilder.AddColumn<bool>(
                name: "TieneCoizacion",
                table: "OrdenTrabajoDetalle",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
