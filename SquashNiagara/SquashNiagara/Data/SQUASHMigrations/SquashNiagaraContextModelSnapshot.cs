﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SquashNiagara.Data;

namespace SquashNiagara.Data.SQUASHMigrations
{
    [DbContext(typeof(SquashNiagaraContext))]
    partial class SquashNiagaraContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("SQUASH")
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SquashNiagara.Models.Division", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<short>("PositionNo");

                    b.HasKey("ID");

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("SquashNiagara.Models.Fixture", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Approved")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<short?>("AwayTeamBonus");

                    b.Property<int>("AwayTeamID");

                    b.Property<short?>("AwayTeamScore");

                    b.Property<int?>("CaptainApproveID");

                    b.Property<int?>("CaptainResultID");

                    b.Property<DateTime>("Date");

                    b.Property<int>("DivisionID");

                    b.Property<short?>("HomeTeamBonus");

                    b.Property<int>("HomeTeamID");

                    b.Property<short?>("HomeTeamScore");

                    b.Property<int>("SeasonID");

                    b.Property<DateTime>("Time");

                    b.Property<int>("VenueID");

                    b.HasKey("ID");

                    b.HasIndex("AwayTeamID");

                    b.HasIndex("CaptainApproveID");

                    b.HasIndex("CaptainResultID");

                    b.HasIndex("HomeTeamID");

                    b.HasIndex("SeasonID");

                    b.HasIndex("VenueID");

                    b.HasIndex("DivisionID", "HomeTeamID", "AwayTeamID", "Date", "Time")
                        .IsUnique();

                    b.ToTable("Fixtures");
                });

            modelBuilder.Entity("SquashNiagara.Models.Match", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AwayPlayerID");

                    b.Property<short?>("AwayPlayerScore");

                    b.Property<int>("FixtureID");

                    b.Property<int>("HomePlayerID");

                    b.Property<short?>("HomePlayerScore");

                    b.HasKey("ID");

                    b.HasIndex("AwayPlayerID");

                    b.HasIndex("HomePlayerID");

                    b.HasIndex("FixtureID", "HomePlayerID", "AwayPlayerID")
                        .IsUnique();

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("SquashNiagara.Models.Player", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DOB");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("TeamID");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("TeamID");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SquashNiagara.Models.PlayerPosition", b =>
                {
                    b.Property<int>("PlayerID");

                    b.Property<int>("PositionID");

                    b.Property<int?>("MatchID");

                    b.HasKey("PlayerID", "PositionID");

                    b.HasIndex("MatchID");

                    b.HasIndex("PositionID");

                    b.ToTable("PlayerPositions");
                });

            modelBuilder.Entity("SquashNiagara.Models.Position", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("SquashNiagara.Models.Season", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("StartDate");

                    b.HasKey("ID");

                    b.HasIndex("StartDate", "EndDate")
                        .IsUnique();

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("SquashNiagara.Models.SeasonDivisionTeam", b =>
                {
                    b.Property<int>("SeasonID");

                    b.Property<int>("DivisionID");

                    b.Property<int>("TeamID");

                    b.HasKey("SeasonID", "DivisionID", "TeamID");

                    b.HasIndex("DivisionID");

                    b.HasIndex("TeamID");

                    b.ToTable("SeasonDivisionTeams");
                });

            modelBuilder.Entity("SquashNiagara.Models.Team", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CaptainID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("VenueID");

                    b.HasKey("ID");

                    b.HasIndex("CaptainID");

                    b.HasIndex("VenueID");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("SquashNiagara.Models.Venue", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(256);

                    b.Property<string>("City")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PostalCode")
                        .HasMaxLength(6);

                    b.Property<string>("Province")
                        .HasMaxLength(2);

                    b.HasKey("ID");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("SquashNiagara.Models.Fixture", b =>
                {
                    b.HasOne("SquashNiagara.Models.Team", "AwayTeam")
                        .WithMany("AwayFixtures")
                        .HasForeignKey("AwayTeamID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Player", "CaptainApprove")
                        .WithMany("FixtureCaptainApproves")
                        .HasForeignKey("CaptainApproveID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Player", "CaptainResult")
                        .WithMany("FixtureCaptainResults")
                        .HasForeignKey("CaptainResultID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Division", "Division")
                        .WithMany("Fixtures")
                        .HasForeignKey("DivisionID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Team", "HomeTeam")
                        .WithMany("HomeFixtures")
                        .HasForeignKey("HomeTeamID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Season", "Season")
                        .WithMany("Fixtures")
                        .HasForeignKey("SeasonID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Venue", "Venue")
                        .WithMany("Fixtures")
                        .HasForeignKey("VenueID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SquashNiagara.Models.Match", b =>
                {
                    b.HasOne("SquashNiagara.Models.Player", "AwayPlayer")
                        .WithMany("AwayMatches")
                        .HasForeignKey("AwayPlayerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Fixture", "Fixture")
                        .WithMany("Matches")
                        .HasForeignKey("FixtureID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Player", "HomePlayer")
                        .WithMany("HomeMatches")
                        .HasForeignKey("HomePlayerID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SquashNiagara.Models.Player", b =>
                {
                    b.HasOne("SquashNiagara.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SquashNiagara.Models.PlayerPosition", b =>
                {
                    b.HasOne("SquashNiagara.Models.Match")
                        .WithMany("PlayerPositions")
                        .HasForeignKey("MatchID");

                    b.HasOne("SquashNiagara.Models.Player", "Player")
                        .WithMany("PlayerPositions")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Position", "Position")
                        .WithMany("PlayerPositions")
                        .HasForeignKey("PositionID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SquashNiagara.Models.SeasonDivisionTeam", b =>
                {
                    b.HasOne("SquashNiagara.Models.Division", "Division")
                        .WithMany("SeasonDivisionTeams")
                        .HasForeignKey("DivisionID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Season", "Season")
                        .WithMany("SeasonDivisionTeams")
                        .HasForeignKey("SeasonID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Team", "Team")
                        .WithMany("SeasonDivisionTeams")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SquashNiagara.Models.Team", b =>
                {
                    b.HasOne("SquashNiagara.Models.Player", "Captain")
                        .WithMany("TeamCaptains")
                        .HasForeignKey("CaptainID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SquashNiagara.Models.Venue", "Venue")
                        .WithMany("Teams")
                        .HasForeignKey("VenueID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
