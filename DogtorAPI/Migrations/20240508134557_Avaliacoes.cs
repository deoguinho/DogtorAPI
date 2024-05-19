using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogtorAPI.Migrations
{
    public partial class Avaliacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    VeterinarioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Veterinario_VeterinarioID",
                        column: x => x.VeterinarioID,
                        principalTable: "Veterinario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_VeterinarioID",
                table: "Avaliacoes",
                column: "VeterinarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");
        }
    }
}
