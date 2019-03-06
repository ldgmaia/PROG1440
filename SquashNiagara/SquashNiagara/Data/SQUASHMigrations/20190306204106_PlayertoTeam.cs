using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class PlayertoTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerTeams",
                schema: "SQUASH");

            migrationBuilder.AddColumn<int>(
                name: "TeamID",
                schema: "SQUASH",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamID",
                schema: "SQUASH",
                table: "Players",
                column: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamID",
                schema: "SQUASH",
                table: "Players",
                column: "TeamID",
                principalSchema: "SQUASH",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TeamID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PlayerTeams",
                schema: "SQUASH",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    TeamID = table.Column<int>(nullable: false),
                    PositionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTeams", x => new { x.PlayerID, x.TeamID, x.PositionID });
                    table.ForeignKey(
                        name: "FK_PlayerTeams_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalSchema: "SQUASH",
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerTeams_Positions_PositionID",
                        column: x => x.PositionID,
                        principalSchema: "SQUASH",
                        principalTable: "Positions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerTeams_Teams_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "SQUASH",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeams_PositionID",
                schema: "SQUASH",
                table: "PlayerTeams",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeams_TeamID",
                schema: "SQUASH",
                table: "PlayerTeams",
                column: "TeamID");
        }
    }
}
