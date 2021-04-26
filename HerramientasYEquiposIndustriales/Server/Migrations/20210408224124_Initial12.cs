using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comentarios",
                table: "OrdenTrabajoDetalle",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "OrdenTrabajoDetalle",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comentarios",
                table: "OrdenTrabajoDetalle");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "OrdenTrabajoDetalle");
        }
    }
}
