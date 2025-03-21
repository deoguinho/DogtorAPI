﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogtorAPI.Migrations
{
    public partial class newfieldVet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Veterinario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Veterinario");
        }
    }
}
