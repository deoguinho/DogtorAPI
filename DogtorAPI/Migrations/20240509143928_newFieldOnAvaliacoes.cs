using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogtorAPI.Migrations
{
    public partial class newFieldOnAvaliacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TutorID",
                table: "Avaliacoes",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_TutorID",
                table: "Avaliacoes",
                column: "TutorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Tutor_TutorID",
                table: "Avaliacoes",
                column: "TutorID",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Tutor_TutorID",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_TutorID",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "TutorID",
                table: "Avaliacoes");
        }
    }
}
