using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class Nullable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "HomePlayerScore",
                schema: "SQUASH",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "AwayPlayerScore",
                schema: "SQUASH",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "HomeTeamScore",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "HomeTeamBonus",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "AwayTeamScore",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "AwayTeamBonus",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(short));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "HomePlayerScore",
                schema: "SQUASH",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "AwayPlayerScore",
                schema: "SQUASH",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "HomeTeamScore",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "HomeTeamBonus",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "AwayTeamScore",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "AwayTeamBonus",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);
        }
    }
}
