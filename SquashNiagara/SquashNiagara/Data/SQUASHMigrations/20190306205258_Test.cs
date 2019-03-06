using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CaptainID",
                schema: "SQUASH",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CaptainID",
                schema: "SQUASH",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
