using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogtorAPI.Migrations
{
    public partial class CreateForeignTutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TutorId",
                table: "Pet",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pet_TutorId",
                table: "Pet",
                column: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Tutor_TutorId",
                table: "Pet",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Tutor_TutorId",
                table: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_Pet_TutorId",
                table: "Pet");

            migrationBuilder.DropColumn(
                name: "TutorId",
                table: "Pet");
        }
    }
}
