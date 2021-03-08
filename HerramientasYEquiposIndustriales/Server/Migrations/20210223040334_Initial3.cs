using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpleadoBaja",
                table: "Productos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Productos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Curp",
                table: "Clientes",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nss",
                table: "Clientes",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpleadoBaja",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Curp",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Nss",
                table: "Clientes");
        }
    }
}
