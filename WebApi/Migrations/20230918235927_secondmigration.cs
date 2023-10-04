using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class secondmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoJuegos",
                table: "VideoJuegos");

            migrationBuilder.RenameTable(
                name: "VideoJuegos",
                newName: "VideoJuego");

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "VideoJuego",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoJuego",
                table: "VideoJuego",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoJuego",
                table: "VideoJuego");

            migrationBuilder.RenameTable(
                name: "VideoJuego",
                newName: "VideoJuegos");

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "VideoJuegos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoJuegos",
                table: "VideoJuegos",
                column: "id");
        }
    }
}
