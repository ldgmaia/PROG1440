using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class PlayerChanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Positions_PositionID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "TeamID",
                schema: "SQUASH",
                table: "Players",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PositionID",
                schema: "SQUASH",
                table: "Players",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Positions_PositionID",
                schema: "SQUASH",
                table: "Players",
                column: "PositionID",
                principalSchema: "SQUASH",
                principalTable: "Positions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Positions_PositionID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "TeamID",
                schema: "SQUASH",
                table: "Players",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionID",
                schema: "SQUASH",
                table: "Players",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Positions_PositionID",
                schema: "SQUASH",
                table: "Players",
                column: "PositionID",
                principalSchema: "SQUASH",
                principalTable: "Positions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
