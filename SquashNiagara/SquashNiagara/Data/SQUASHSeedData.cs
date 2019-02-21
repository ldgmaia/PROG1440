using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SquashNiagara.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SquashNiagara.Data
{
    public static class SQUASHSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SquashNiagaraContext(
                serviceProvider.GetRequiredService<DbContextOptions<SquashNiagaraContext>>()))
            {
                //Add sample data Table Season
                if (!context.Seasons.Any())
                {
                    context.Seasons.AddRange(
                        new Season
                        {
                            Name = "2018-19",
                            StartDate = DateTime.Parse("2018-10-01"),
                            EndDate = DateTime.Parse("2019-04-01")
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table Division
                if (!context.Divisions.Any())
                {
                    context.Divisions.AddRange(
                        new Division
                        {
                            Name = "Men's Division 1/2",
                            PositionNo = 4
                        },
                        new Division
                        {
                            Name = "Men's Division 3",
                            PositionNo = 4
                        },
                        new Division
                        {
                            Name = "Men's Division 4",
                            PositionNo = 4
                        },
                        new Division
                        {
                            Name = "Woment's Division",
                            PositionNo = 5
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table Position
                if (!context.Positions.Any())
                {
                    context.Positions.AddRange(
                        new Position
                        {
                            Name = "Position 1"
                        },
                        new Position
                        {
                            Name = "Position 2"
                        },
                        new Position
                        {
                            Name = "Position 3"
                        },
                        new Position
                        {
                            Name = "Position 4"
                        },
                        new Position
                        {
                            Name = "Position 5"
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table Player
                if (!context.Players.Any())
                {
                    context.Players.AddRange(
                        new Player
                        {
                            FirstName = "Kok-Wah",
                            LastName = "Seet",
                            Email = "Seet@home.com"
                        },
                         new Player
                         {
                             FirstName = "George",
                             LastName = "Kelm",
                             Email = "Kelm@home.com"
                         }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table Venue
                if (!context.Venues.Any())
                {
                    context.Venues.AddRange(
                        new Venue
                        {
                            Name = "BAC"
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table Team
                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                        new Team
                        {
                            Name = "BAC 1",
                            VenueID = 1,
                            CaptainID = 1
                        },
                        new Team
                        {
                            Name = "BAC 2",
                            VenueID = 1,
                            CaptainID = 2
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table SeasonDivisionTeam
                if (!context.SeasonDivisionTeams.Any())
                {
                    context.SeasonDivisionTeams.AddRange(
                        new SeasonDivisionTeam
                        {
                            SeasonID = 1,
                            DivisionID = 1,
                            TeamID = 1 
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table PlayerTeam
                if (!context.PlayerTeams.Any())
                {
                    context.PlayerTeams.AddRange(
                        new PlayerTeam
                        {
                            TeamID = 1,
                            PlayerID = 1,
                            PositionID = 1
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table Fixture
                if (!context.Fixtures.Any())
                {
                    context.Fixtures.AddRange(
                        new Fixture
                        {
                            DivisionID = 1,
                            HomeTeamID = 1,
                            AwayTeamID = 2,
                            Date = DateTime.Parse("2019-19-02"),
                            Time = DateTime.Parse("18:00"),
                            VenueID = 1
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table Match
                if (!context.Matches.Any())
                {
                    context.Matches.AddRange(
                        new Match
                        {
                            FixtureID = 1,
                            HomePlayerID = 1,
                            AwayPlayerID = 2
                        }
                    );
                    context.SaveChanges();
                }

                //Add sample data Table PlayerPosition
                if (!context.PlayerPositions.Any())
                {
                    context.PlayerPositions.AddRange(
                        new PlayerPosition
                        {
                            PlayerID = 1,
                            MatchID = 1,
                            PositionID = 1
                        }
                    );
                    context.SaveChanges();
                }

            }
        }

    }
}
