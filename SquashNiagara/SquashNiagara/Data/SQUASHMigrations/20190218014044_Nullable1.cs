using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class Nullable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CaptainResultID",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CaptainApproveID",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CaptainResultID",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CaptainApproveID",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                schema: "SQUASH",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
