using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class FixtureDivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonID",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_SeasonID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "SeasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Seasons_SeasonID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "SeasonID",
                principalSchema: "SQUASH",
                principalTable: "Seasons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Seasons_SeasonID",
                schema: "SQUASH",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_SeasonID",
                schema: "SQUASH",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "SeasonID",
                schema: "SQUASH",
                table: "Fixtures");
        }
    }
}
