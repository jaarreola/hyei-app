using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Curp",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Nss",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "Curp",
                table: "Empleados",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nss",
                table: "Empleados",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Curp",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "Nss",
                table: "Empleados");

            migrationBuilder.AddColumn<string>(
                name: "Curp",
                table: "Clientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nss",
                table: "Clientes",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
