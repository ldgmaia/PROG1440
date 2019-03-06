using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class Test133 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerPositions",
                schema: "SQUASH",
                table: "PlayerPositions");

            migrationBuilder.AlterColumn<int>(
                name: "MatchID",
                schema: "SQUASH",
                table: "PlayerPositions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerPositions",
                schema: "SQUASH",
                table: "PlayerPositions",
                columns: new[] { "PlayerID", "PositionID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerPositions",
                schema: "SQUASH",
                table: "PlayerPositions");

            migrationBuilder.AlterColumn<int>(
                name: "MatchID",
                schema: "SQUASH",
                table: "PlayerPositions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerPositions",
                schema: "SQUASH",
                table: "PlayerPositions",
                columns: new[] { "PlayerID", "MatchID", "PositionID" });
        }
    }
}
