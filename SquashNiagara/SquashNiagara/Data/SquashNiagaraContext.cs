using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SquashNiagara.Models;

namespace SquashNiagara.Data
{
    public class SquashNiagaraContext : DbContext
    {
        public SquashNiagaraContext(DbContextOptions<SquashNiagaraContext> options)
            : base(options)
        {
        }

        public DbSet<Season> Seasons { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<SeasonDivisionTeam> SeasonDivisionTeams { get; set; }
        //public DbSet<PlayerTeam> PlayerTeams { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SQUASH");

            //Many to Many Primary Key
            modelBuilder.Entity<SeasonDivisionTeam>()
            .HasKey(p => new { p.SeasonID, p.DivisionID, p.TeamID });

            ////Many to Many Primary Key
            //modelBuilder.Entity<PlayerTeam>()
            //.HasKey(p => new { p.PlayerID, p.TeamID, p.PositionID});

            //Many to Many Primary Key
            modelBuilder.Entity<PlayerPosition>()
            .HasKey(p => new { p.PlayerID, p.MatchID, p.PositionID });

            //Add a unique index to the Player e-Mail
            modelBuilder.Entity<Player>()
            .HasIndex(p => p.Email)
            .IsUnique();

            //Add a unique index to the Season 
            //StartDate and EndDate
            modelBuilder.Entity<Season>()
            .HasIndex(a => new { a.StartDate, a.EndDate })
            .IsUnique();

            //Add a unique index to the Fixture
            //Divison, Team Home, Team Away, Date, Time
            modelBuilder.Entity<Fixture>()
            .HasIndex(a => new { a.DivisionID, a.HomeTeamID, a.AwayTeamID, a.Date, a.Time })
            .IsUnique();

            //Add a unique index to the Match
            //FixtureID, HomePlayerID, AwayPlayerID
            modelBuilder.Entity<Match>()
            .HasIndex(a => new { a.FixtureID, a.HomePlayerID, a.AwayPlayerID })
            .IsUnique();

            //Prevent Cascade Delete Season to SeasonDivisionTeam
            modelBuilder.Entity<Season>()
                .HasMany<SeasonDivisionTeam>(p => p.SeasonDivisionTeams)
                .WithOne(c => c.Season)
                .HasForeignKey(c => c.SeasonID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Division to SeasonDivisionTeam
            modelBuilder.Entity<Division>()
                .HasMany<SeasonDivisionTeam>(p => p.SeasonDivisionTeams)
                .WithOne(c => c.Division)
                .HasForeignKey(c => c.DivisionID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Team to SeasonDivisionTeam
            modelBuilder.Entity<Team>()
                .HasMany<SeasonDivisionTeam>(p => p.SeasonDivisionTeams)
                .WithOne(c => c.Team)
                .HasForeignKey(c => c.TeamID)
                .OnDelete(DeleteBehavior.Restrict);

            ////Prevent Cascade Delete Player to PlayerTeam
            //modelBuilder.Entity<Player>()
            //    .HasMany<PlayerTeam>(p => p.PlayerTeams)
            //    .WithOne(c => c.Player)
            //    .HasForeignKey(c => c.PlayerID)
            //    .OnDelete(DeleteBehavior.Restrict);

            ////Prevent Cascade Delete Team to PlayerTeam
            //modelBuilder.Entity<Team>()
            //    .HasMany<PlayerTeam>(p => p.PlayerTeams)
            //    .WithOne(c => c.Team)
            //    .HasForeignKey(c => c.TeamID)
            //    .OnDelete(DeleteBehavior.Restrict);

            ////Prevent Cascade Delete Position to PlayerTeam
            //modelBuilder.Entity<Position>()
            //    .HasMany<PlayerTeam>(p => p.PlayerTeams)
            //    .WithOne(c => c.Position)
            //    .HasForeignKey(c => c.PositionID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Player to Team
            modelBuilder.Entity<Team>()
                .HasMany<Player>(p => p.Players)
                .WithOne(c => c.Team)
                .HasForeignKey(c => c.TeamID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Player to PlayerPosition
            //modelBuilder.Entity<Player>()
            //    .HasMany<PlayerPosition>(p => p.PlayerPositions)
            //    .WithOne(c => c.Player)
            //    .HasForeignKey(c => c.PlayerID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Position to PlayerPosition
            modelBuilder.Entity<Position>()
                .HasMany<PlayerPosition>(p => p.PlayerPositions)
                .WithOne(c => c.Position)
                .HasForeignKey(c => c.PositionID)
                .OnDelete(DeleteBehavior.Restrict);

            ////Prevent Cascade Delete Match to PlayerPosition
            //modelBuilder.Entity<Match>()
            //    .HasMany<PlayerPosition>(p => p.PlayerPositions)
            //    .WithOne(c => c.Match)
            //    .HasForeignKey(c => c.MatchID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Venue to Team
            modelBuilder.Entity<Venue>()
                .HasMany<Team>(p => p.Teams)
                .WithOne(c => c.Venue)
                .HasForeignKey(c => c.VenueID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Venue to Fixture
            modelBuilder.Entity<Venue>()
                .HasMany<Fixture>(p => p.Fixtures)
                .WithOne(c => c.Venue)
                .HasForeignKey(c => c.VenueID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Division to Fixture
            modelBuilder.Entity<Division>()
                .HasMany<Fixture>(p => p.Fixtures)
                .WithOne(c => c.Division)
                .HasForeignKey(c => c.DivisionID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Season to Fixture
            modelBuilder.Entity<Season>()
                .HasMany<Fixture>(p => p.Fixtures)
                .WithOne(c => c.Season)
                .HasForeignKey(c => c.SeasonID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Fixture to Match
            modelBuilder.Entity<Fixture>()
                .HasMany<Match>(p => p.Matches)
                .WithOne(c => c.Fixture)
                .HasForeignKey(c => c.FixtureID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Team (home) to Fixture
            modelBuilder.Entity<Team>()
                .HasMany<Fixture>(p => p.HomeFixtures)
                .WithOne(c => c.HomeTeam)
                .HasForeignKey(c => c.HomeTeamID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Team (away) to Fixture
            modelBuilder.Entity<Team>()
                .HasMany<Fixture>(p => p.AwayFixtures)
                .WithOne(c => c.AwayTeam)
                .HasForeignKey(c => c.AwayTeamID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Player (home) to Match
            modelBuilder.Entity<Player>()
                .HasMany<Match>(p => p.HomeMatches)
                .WithOne(c => c.HomePlayer)
                .HasForeignKey(c => c.HomePlayerID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Player (away) to Match
            modelBuilder.Entity<Player>()
                .HasMany<Match>(p => p.AwayMatches)
                .WithOne(c => c.AwayPlayer)
                .HasForeignKey(c => c.AwayPlayerID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Player (captain) to Team
            //modelBuilder.Entity<Player>()
            //    .HasMany<Team>(p => p.TeamCaptains)
            //    .WithOne(c => c.Captain)
            //    .HasForeignKey(c => c.CaptainID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Player (CaptainResult) to Fixture
            modelBuilder.Entity<Player>()
                .HasMany<Fixture>(p => p.FixtureCaptainResults)
                .WithOne(c => c.CaptainResult)
                .HasForeignKey(c => c.CaptainResultID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete Player (CaptainApprove) to Fixture
            modelBuilder.Entity<Player>()
                .HasMany<Fixture>(p => p.FixtureCaptainApproves)
                .WithOne(c => c.CaptainApprove)
                .HasForeignKey(c => c.CaptainApproveID)
                .OnDelete(DeleteBehavior.Restrict);

            //Add one-to-many
            modelBuilder.Entity<Fixture>(e =>
            {
                e.HasOne(r => r.HomeTeam).WithMany(r => r.HomeFixtures);
                e.HasOne(r => r.AwayTeam).WithMany(r => r.AwayFixtures);
                e.HasOne(r => r.CaptainResult).WithMany(r => r.FixtureCaptainResults);
                e.HasOne(r => r.CaptainApprove).WithMany(r => r.FixtureCaptainApproves);
            });

            //Add one-to-many
            modelBuilder.Entity<Match>(e =>
            {
                e.HasOne(r => r.HomePlayer).WithMany(r => r.HomeMatches);
                e.HasOne(r => r.AwayPlayer).WithMany(r => r.AwayMatches);
            });

            modelBuilder.Entity<Fixture>()
            .Property(b => b.Approved)
            .HasDefaultValue(bool.Parse("False"));
        }

    }
}
