using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class clientes : Migration
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
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
