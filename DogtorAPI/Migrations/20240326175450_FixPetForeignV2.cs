using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogtorAPI.Migrations
{
    public partial class FixPetForeignV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Tutor_TutorId",
                table: "Pet");

            migrationBuilder.RenameColumn(
                name: "TutorId",
                table: "Pet",
                newName: "TutorID");

            migrationBuilder.RenameIndex(
                name: "IX_Pet_TutorId",
                table: "Pet",
                newName: "IX_Pet_TutorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Tutor_TutorID",
                table: "Pet",
                column: "TutorID",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Tutor_TutorID",
                table: "Pet");

            migrationBuilder.RenameColumn(
                name: "TutorID",
                table: "Pet",
                newName: "TutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Pet_TutorID",
                table: "Pet",
                newName: "IX_Pet_TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Tutor_TutorId",
                table: "Pet",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
