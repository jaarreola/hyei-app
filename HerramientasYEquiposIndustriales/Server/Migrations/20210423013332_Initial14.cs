using Microsoft.EntityFrameworkCore.Migrations;

namespace HerramientasYEquiposIndustriales.Server.Migrations
{
    public partial class Initial14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "EstatusOTFlujos",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comentario",
                table: "EstatusOTFlujos",
                maxLength: 5000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comentario",
                table: "EstatusOTFlujos");

            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "EstatusOTFlujos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
