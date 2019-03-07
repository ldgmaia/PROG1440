using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class PlayerChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPositions_Matches_MatchID",
                schema: "SQUASH",
                table: "PlayerPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPositions_Players_PlayerID",
                schema: "SQUASH",
                table: "PlayerPositions");

            migrationBuilder.AddColumn<int>(
                name: "PositionID",
                schema: "SQUASH",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MatchID",
                schema: "SQUASH",
                table: "PlayerPositions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_PositionID",
                schema: "SQUASH",
                table: "Players",
                column: "PositionID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPositions_Matches_MatchID",
                schema: "SQUASH",
                table: "PlayerPositions",
                column: "MatchID",
                principalSchema: "SQUASH",
                principalTable: "Matches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPositions_Players_PlayerID",
                schema: "SQUASH",
                table: "PlayerPositions",
                column: "PlayerID",
                principalSchema: "SQUASH",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPositions_Matches_MatchID",
                schema: "SQUASH",
                table: "PlayerPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPositions_Players_PlayerID",
                schema: "SQUASH",
                table: "PlayerPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Positions_PositionID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_PositionID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PositionID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "MatchID",
                schema: "SQUASH",
                table: "PlayerPositions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPositions_Matches_MatchID",
                schema: "SQUASH",
                table: "PlayerPositions",
                column: "MatchID",
                principalSchema: "SQUASH",
                principalTable: "Matches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPositions_Players_PlayerID",
                schema: "SQUASH",
                table: "PlayerPositions",
                column: "PlayerID",
                principalSchema: "SQUASH",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
