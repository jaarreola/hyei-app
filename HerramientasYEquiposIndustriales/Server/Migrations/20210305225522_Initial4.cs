using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpleadoActivo",
                table: "Productos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActivo",
                table: "Productos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacturaMovimientos_Factura",
                table: "FacturaMovimientos",
                column: "Factura",
                unique: true,
                filter: "[Factura] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FacturaMovimientos_Factura",
                table: "FacturaMovimientos");

            migrationBuilder.DropColumn(
                name: "EmpleadoActivo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaActivo",
                table: "Productos");
        }
    }
}
