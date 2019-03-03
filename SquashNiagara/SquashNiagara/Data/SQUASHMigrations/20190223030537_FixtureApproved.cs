using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class FixtureApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: false);
        }
    }
}
