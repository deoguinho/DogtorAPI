using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogtorAPI.Migrations
{
    public partial class changeTypeNameEspecialidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Veterinario");

            migrationBuilder.RenameColumn(
                name: "VeteriarioID",
                table: "Especialidade",
                newName: "VeterinarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Especialidade_VeterinarioID",
                table: "Especialidade",
                column: "VeterinarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Especialidade_Veterinario_VeterinarioID",
                table: "Especialidade",
                column: "VeterinarioID",
                principalTable: "Veterinario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Especialidade_Veterinario_VeterinarioID",
                table: "Especialidade");

            migrationBuilder.DropIndex(
                name: "IX_Especialidade_VeterinarioID",
                table: "Especialidade");

            migrationBuilder.RenameColumn(
                name: "VeterinarioID",
                table: "Especialidade",
                newName: "VeteriarioID");

            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                table: "Veterinario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
