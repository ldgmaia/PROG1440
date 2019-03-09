using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SquashNiagara.Data.SQUASHMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SQUASH");

            migrationBuilder.CreateTable(
                name: "Divisions",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PositionNo = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 256, nullable: true),
                    City = table.Column<string>(maxLength: 256, nullable: true),
                    Province = table.Column<string>(maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Fixtures",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SeasonID = table.Column<int>(nullable: false),
                    DivisionID = table.Column<int>(nullable: false),
                    HomeTeamID = table.Column<int>(nullable: false),
                    AwayTeamID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    VenueID = table.Column<int>(nullable: false),
                    HomeTeamScore = table.Column<short>(nullable: true),
                    AwayTeamScore = table.Column<short>(nullable: true),
                    HomeTeamBonus = table.Column<short>(nullable: true),
                    AwayTeamBonus = table.Column<short>(nullable: true),
                    CaptainResultID = table.Column<int>(nullable: true),
                    CaptainApproveID = table.Column<int>(nullable: true),
                    Approved = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Fixtures_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalSchema: "SQUASH",
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Seasons_SeasonID",
                        column: x => x.SeasonID,
                        principalSchema: "SQUASH",
                        principalTable: "Seasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Venues_VenueID",
                        column: x => x.VenueID,
                        principalSchema: "SQUASH",
                        principalTable: "Venues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeasonDivisionTeams",
                schema: "SQUASH",
                columns: table => new
                {
                    SeasonID = table.Column<int>(nullable: false),
                    DivisionID = table.Column<int>(nullable: false),
                    TeamID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonDivisionTeams", x => new { x.SeasonID, x.DivisionID, x.TeamID });
                    table.ForeignKey(
                        name: "FK_SeasonDivisionTeams_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalSchema: "SQUASH",
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeasonDivisionTeams_Seasons_SeasonID",
                        column: x => x.SeasonID,
                        principalSchema: "SQUASH",
                        principalTable: "Seasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FixtureID = table.Column<int>(nullable: false),
                    HomePlayerID = table.Column<int>(nullable: false),
                    AwayPlayerID = table.Column<int>(nullable: false),
                    HomePlayerScore = table.Column<short>(nullable: true),
                    AwayPlayerScore = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Matches_Fixtures_FixtureID",
                        column: x => x.FixtureID,
                        principalSchema: "SQUASH",
                        principalTable: "Fixtures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerPositions",
                schema: "SQUASH",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    MatchID = table.Column<int>(nullable: false),
                    PositionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPositions", x => new { x.PlayerID, x.MatchID, x.PositionID });
                    table.ForeignKey(
                        name: "FK_PlayerPositions_Matches_MatchID",
                        column: x => x.MatchID,
                        principalSchema: "SQUASH",
                        principalTable: "Matches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerPositions_Positions_PositionID",
                        column: x => x.PositionID,
                        principalSchema: "SQUASH",
                        principalTable: "Positions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CaptainID = table.Column<int>(nullable: true),
                    VenueID = table.Column<int>(nullable: true),
                    imageContent = table.Column<byte[]>(nullable: true),
                    imageMimeType = table.Column<string>(maxLength: 256, nullable: true),
                    imageFileName = table.Column<string>(maxLength: 100, nullable: true),
                    Profile = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Teams_Venues_VenueID",
                        column: x => x.VenueID,
                        principalSchema: "SQUASH",
                        principalTable: "Venues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                schema: "SQUASH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    DOB = table.Column<DateTime>(nullable: true),
                    imageContent = table.Column<byte[]>(nullable: true),
                    imageMimeType = table.Column<string>(maxLength: 256, nullable: true),
                    imageFileName = table.Column<string>(maxLength: 100, nullable: true),
                    TeamID = table.Column<int>(nullable: true),
                    PositionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Players_Positions_PositionID",
                        column: x => x.PositionID,
                        principalSchema: "SQUASH",
                        principalTable: "Positions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "SQUASH",
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_AwayTeamID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "AwayTeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_CaptainApproveID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "CaptainApproveID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_CaptainResultID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "CaptainResultID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_HomeTeamID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "HomeTeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_SeasonID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "SeasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_VenueID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "VenueID");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_DivisionID_HomeTeamID_AwayTeamID_Date_Time",
                schema: "SQUASH",
                table: "Fixtures",
                columns: new[] { "DivisionID", "HomeTeamID", "AwayTeamID", "Date", "Time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayPlayerID",
                schema: "SQUASH",
                table: "Matches",
                column: "AwayPlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomePlayerID",
                schema: "SQUASH",
                table: "Matches",
                column: "HomePlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FixtureID_HomePlayerID_AwayPlayerID",
                schema: "SQUASH",
                table: "Matches",
                columns: new[] { "FixtureID", "HomePlayerID", "AwayPlayerID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPositions_MatchID",
                schema: "SQUASH",
                table: "PlayerPositions",
                column: "MatchID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPositions_PositionID",
                schema: "SQUASH",
                table: "PlayerPositions",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Email",
                schema: "SQUASH",
                table: "Players",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_PositionID",
                schema: "SQUASH",
                table: "Players",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamID",
                schema: "SQUASH",
                table: "Players",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonDivisionTeams_DivisionID",
                schema: "SQUASH",
                table: "SeasonDivisionTeams",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonDivisionTeams_TeamID",
                schema: "SQUASH",
                table: "SeasonDivisionTeams",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_StartDate_EndDate",
                schema: "SQUASH",
                table: "Seasons",
                columns: new[] { "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CaptainID",
                schema: "SQUASH",
                table: "Teams",
                column: "CaptainID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_VenueID",
                schema: "SQUASH",
                table: "Teams",
                column: "VenueID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_AwayTeamID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "AwayTeamID",
                principalSchema: "SQUASH",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_HomeTeamID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "HomeTeamID",
                principalSchema: "SQUASH",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Players_CaptainApproveID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "CaptainApproveID",
                principalSchema: "SQUASH",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Players_CaptainResultID",
                schema: "SQUASH",
                table: "Fixtures",
                column: "CaptainResultID",
                principalSchema: "SQUASH",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonDivisionTeams_Teams_TeamID",
                schema: "SQUASH",
                table: "SeasonDivisionTeams",
                column: "TeamID",
                principalSchema: "SQUASH",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_AwayPlayerID",
                schema: "SQUASH",
                table: "Matches",
                column: "AwayPlayerID",
                principalSchema: "SQUASH",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_HomePlayerID",
                schema: "SQUASH",
                table: "Matches",
                column: "HomePlayerID",
                principalSchema: "SQUASH",
                principalTable: "Players",
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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Players_CaptainID",
                schema: "SQUASH",
                table: "Teams",
                column: "CaptainID",
                principalSchema: "SQUASH",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamID",
                schema: "SQUASH",
                table: "Players");

            migrationBuilder.DropTable(
                name: "PlayerPositions",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "SeasonDivisionTeams",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Matches",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Fixtures",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Divisions",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Seasons",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Players",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Venues",
                schema: "SQUASH");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "SQUASH");
        }
    }
}
