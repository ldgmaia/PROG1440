using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class players_pictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "imageContent",
                schema: "SQUASH",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageFileName",
                schema: "SQUASH",
                table: "Players",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageMimeType",
                schema: "SQUASH",
                table: "Players",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageContent",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "imageFileName",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "imageMimeType",
                schema: "SQUASH",
                table: "Players");
        }
    }
}
