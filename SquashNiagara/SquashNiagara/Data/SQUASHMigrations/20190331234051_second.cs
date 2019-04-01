using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Fixtures_FixtureID",
                schema: "SQUASH",
                table: "Matches");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Fixtures_FixtureID",
                schema: "SQUASH",
                table: "Matches",
                column: "FixtureID",
                principalSchema: "SQUASH",
                principalTable: "Fixtures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Fixtures_FixtureID",
                schema: "SQUASH",
                table: "Matches");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Fixtures_FixtureID",
                schema: "SQUASH",
                table: "Matches",
                column: "FixtureID",
                principalSchema: "SQUASH",
                principalTable: "Fixtures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
