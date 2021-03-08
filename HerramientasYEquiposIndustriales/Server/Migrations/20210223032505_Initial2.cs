using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimientos_Empleados_EmpleadoCreaEmpleadoId",
                table: "Movimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimientos_Empleados_EmpleadoModificaEmpleadoId",
                table: "Movimientos");

            migrationBuilder.DropIndex(
                name: "IX_Movimientos_EmpleadoCreaEmpleadoId",
                table: "Movimientos");

            migrationBuilder.DropIndex(
                name: "IX_Movimientos_EmpleadoModificaEmpleadoId",
                table: "Movimientos");

            migrationBuilder.DropColumn(
                name: "EmpleadoCreaEmpleadoId",
                table: "Movimientos");

            migrationBuilder.DropColumn(
                name: "EmpleadoModificaEmpleadoId",
                table: "Movimientos");

            migrationBuilder.AddColumn<float>(
                name: "CantidadMinimaInventario",
                table: "Productos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_NoParte",
                table: "Productos",
                column: "NoParte",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Productos_NoParte",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CantidadMinimaInventario",
                table: "Productos");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoCreaEmpleadoId",
                table: "Movimientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoModificaEmpleadoId",
                table: "Movimientos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_EmpleadoCreaEmpleadoId",
                table: "Movimientos",
                column: "EmpleadoCreaEmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_EmpleadoModificaEmpleadoId",
                table: "Movimientos",
                column: "EmpleadoModificaEmpleadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_Empleados_EmpleadoCreaEmpleadoId",
                table: "Movimientos",
                column: "EmpleadoCreaEmpleadoId",
                principalTable: "Empleados",
                principalColumn: "EmpleadoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_Empleados_EmpleadoModificaEmpleadoId",
                table: "Movimientos",
                column: "EmpleadoModificaEmpleadoId",
                principalTable: "Empleados",
                principalColumn: "EmpleadoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
