using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class FixtureApproved2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }
    }
}
