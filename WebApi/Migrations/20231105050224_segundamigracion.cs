using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class segundamigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoJuego_Desarrollador_desarrolladorId",
                table: "VideoJuego");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoJuego_Desarrollador_desarrolladorId",
                table: "VideoJuego",
                column: "desarrolladorId",
                principalTable: "Desarrollador",
                principalColumn: "desarrolladorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoJuego_Desarrollador_desarrolladorId",
                table: "VideoJuego");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoJuego_Desarrollador_desarrolladorId",
                table: "VideoJuego",
                column: "desarrolladorId",
                principalTable: "Desarrollador",
                principalColumn: "desarrolladorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
