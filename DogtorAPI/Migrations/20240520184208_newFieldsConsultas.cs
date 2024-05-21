using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogtorAPI.Migrations
{
    public partial class newFieldsConsultas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hour",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Consulta");
        }
    }
}
