using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class puestos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    PuestoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 80, nullable: false),
                    EsAdministrador = table.Column<bool>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.PuestoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Puestos");
        }
    }
}
