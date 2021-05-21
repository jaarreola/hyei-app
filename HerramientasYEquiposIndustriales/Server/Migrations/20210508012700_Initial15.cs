using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsProblema",
                table: "Clientes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsProblema",
                table: "Clientes");
        }
    }
}
