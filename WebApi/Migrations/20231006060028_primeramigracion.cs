using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApi.Migrations
{
    public partial class primeramigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Desarrollador",
                columns: table => new
                {
                    desarrolladorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desarrollador", x => x.desarrolladorId);
                });

            migrationBuilder.CreateTable(
                name: "VideoJuego",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    año = table.Column<int>(type: "integer", nullable: false),
                    desarrolladorId = table.Column<int>(type: "integer", nullable: false),
                    peso = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoJuego", x => x.id);
                    table.ForeignKey(
                        name: "FK_VideoJuego_Desarrollador_desarrolladorId",
                        column: x => x.desarrolladorId,
                        principalTable: "Desarrollador",
                        principalColumn: "desarrolladorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoJuego_desarrolladorId",
                table: "VideoJuego",
                column: "desarrolladorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoJuego");

            migrationBuilder.DropTable(
                name: "Desarrollador");
        }
    }
}
