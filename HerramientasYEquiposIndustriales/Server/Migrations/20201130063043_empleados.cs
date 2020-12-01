using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class empleados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    EmpleadoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroEmpleado = table.Column<string>(maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(maxLength: 200, nullable: true),
                    Telefono = table.Column<string>(maxLength: 10, nullable: true),
                    PuestoId = table.Column<int>(nullable: false),
                    Activo = table.Column<bool>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    FechaBaja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.EmpleadoId);
                    table.ForeignKey(
                        name: "FK_Empleados_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "PuestoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_NumeroEmpleado",
                table: "Empleados",
                column: "NumeroEmpleado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_PuestoId",
                table: "Empleados",
                column: "PuestoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleados");
        }
    }
}
