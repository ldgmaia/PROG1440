using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SquashNiagara.Data;
using SquashNiagara.Models;
using SquashNiagara.ViewModels;

namespace SquashNiagara.Controllers
{
    public class HomeController : Controller
    {
        private readonly SquashNiagaraContext _context;

        public HomeController(SquashNiagaraContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Captain") || User.IsInRole("User"))
            {
                var player = _context.Players
                .FirstOrDefault(m => m.Email == User.Identity.Name);

                if (player != null && player.firstLogin)
                    return Redirect("identity/Account/Manage/ChangePassword");

                return RedirectToAction("Details", "Players", new { id = player.ID });
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        //public IActionResult TeamRanking()
        //{
        //    return View();
        //}

        //public IActionResult PlayerRanking()
        //{
        //    return View();
        //}
        // GET: Matches
        public async Task<IActionResult> PlayerRanking(int? SeasonID, string DivisionID)
        {
            //var playerRankings = _context.PlayerRankings.Include(p => p.Player).Include(p => p.Season).Include(p => p.Division);

            var playerRankings = from f in _context.PlayerRankings.
                                 Include(f => f.Player)
                                 .Include(f => f.Season)
                                 .Include(f => f.Division)
                                 select f;

            if (SeasonID.HasValue)
            {
                playerRankings = playerRankings.Where(p => p.SeasonID == SeasonID);
            }
            else
            {
                SeasonID = ((from d in _context.Seasons
                             orderby d.EndDate descending
                             select d.ID).FirstOrDefault());
            }

            if (DivisionID != null)
            {
                playerRankings = playerRankings.Where(p => p.DivisionID == Convert.ToInt32(DivisionID));
            }
            else
            {
                playerRankings = playerRankings.Where(p => p.DivisionID == 0);
            }

            PopulateDropDownListSeason();

            ViewData["DivisionID"] = from d in _context.Divisions
                                     select d;

            return View(await playerRankings.ToListAsync());
        }

        public async Task<IActionResult> TeamRanking(int? SeasonID, string DivisionID)
        {
            //var teamRankings = _context.TeamRankings.Include(p => p.Team).Include(p => p.Season).Include(p => p.Division);

            var teamRankings = from f in _context.TeamRankings
                               .Include(f => f.Team)
                               .Include(f => f.Season)
                               .Include(f => f.Division)
                               select f;

            if (SeasonID.HasValue)
            {
                teamRankings = teamRankings.Where(p => p.SeasonID == SeasonID);
            }
            else
            {
                SeasonID = ((from d in _context.Seasons
                             orderby d.EndDate descending
                             select d.ID).FirstOrDefault());
            }

            if (DivisionID != null)
            {
                teamRankings = teamRankings.Where(p => p.DivisionID == Convert.ToInt32(DivisionID));
            }
            else
            {
                teamRankings = teamRankings.Where(p => p.DivisionID == 0);
            }

            PopulateDropDownListSeason();

            ViewData["DivisionID"] = from d in _context.Divisions
                                     select d;

            return View(await teamRankings.ToListAsync());
        }

        public async Task<IActionResult> Standings(int? SeasonID, string DivisionID)
        {
            ViewBag.error = "";
            if (!SeasonID.HasValue)
            {
                ViewData["FirstTime"] = true;
            }
            else
            {
                ViewData["FirstTime"] = false;
            }
            try
            {
                //Getting the number of positions by division - I'm using hardcoded Division 1
                var nPositions = _context.Divisions.Include(p => p.PositionNo);
                var pos = (from p in nPositions
                           where p.ID == 1
                           select p.PositionNo).SingleOrDefault();

                //Getting a list of all players
                var players = from p in _context.Players
                              where p.ID == 0
                              select p;

                if (SeasonID.HasValue)
                {
                    players = from p in _context.Players
                                  join t in _context.Teams on p.TeamID equals t.ID
                                  join sdt in _context.SeasonDivisionTeams on t.ID equals sdt.TeamID
                                  where sdt.DivisionID == Convert.ToInt32(DivisionID) && sdt.SeasonID == SeasonID
                                  select p;
                }

                

                List<PositionalStandings> positionalStandingsList = new List<PositionalStandings>();

                for (int i = 0; i < pos; i++)
                {
                    foreach (var player in players)
                    {
                        if (player.ID == 1)
                            continue;

                        PositionalStandings p = fixturePlayedByPlayer(player.ID, i + 1, SeasonID, DivisionID);

                        if(p.PercPlayed >= 50)
                            positionalStandingsList.Add(p);
                    }
                }

                ViewBag.positions = pos;
                ViewBag.playersByPosition = positionalStandingsList.OrderByDescending(ps => ps.winPerc).ToList();
                //ViewBag.playersByPosition = positionalStandingsList;
            }
            catch (ArgumentOutOfRangeException err)
            {
                ViewBag.error = "No matches have been played. Add fixture results first.";
            } catch(Exception err)
            {
                ViewBag.error = "Some problem happend that need to be analysed. Report it to the team";
            }
            PopulateDropDownListSeason();

            int userDivision = 0;

            ViewData["UserDivision"] = userDivision;

            ViewData["DivisionID"] = from d in _context.Divisions
                                     select d;

            return View();
        }

        private PositionalStandings fixturePlayedByPlayer(int playerID, int positionID, int? SeasonID, string DivisionID)
        {
            PositionalStandings ps = new PositionalStandings();
            /*
             * getting the amount of fixtures the given player played and the given position 
             */
            var playerPositions = _context.PlayerPositions.Include(pp => pp.PlayerID);
            var players = _context.Players.Include(p => p.ID);

            var fixturesByPlayer = (from pp in playerPositions
                                    join p in players on pp.PlayerID equals p.ID
                                    where p.ID == playerID
                                    && pp.PositionID == positionID
                                    select pp.PositionID).Count();

            /*
             * getting the temaID of the given playerID
             */
            var player = _context.Players.Include(p => p.ID);
            var playerTeamID = (from p in player
                               where p.ID == playerID
                               select new { p.TeamID, p.FirstName, p.LastName}).FirstOrDefault();

            /*
             * getting the amount of fixture the team of the given playerID played as Home
             */
            var fixturesHome = _context.Fixtures.Include(f => f.HomeTeamID);
            var seadivteaHome = _context.SeasonDivisionTeams.Include(sdt => sdt.TeamID);            

            //var teamPlayedHome = from f in fixtures as HomeTeam
            var teamPlayedHome = (from f in fixturesHome
                                  join sdt1 in seadivteaHome on f.HomeTeamID equals sdt1.TeamID
                                  where sdt1.TeamID == playerTeamID.TeamID
                                    select f.ID).Count();
            /*
             * getting the amount of fixture the team of the given playerID played as Away
             */
            var fixturesAway = _context.Fixtures.Include(f => f.HomeTeamID);
            var seadivteaAway = _context.SeasonDivisionTeams.Include(sdt => sdt.TeamID);

            //var teamPlayedHome = from f in fixtures as AwayTeam
            var teamPlayedAway = (from f in fixturesAway
                                  join sdt2 in seadivteaAway on f.AwayTeamID equals sdt2.TeamID
                                  where sdt2.TeamID == playerTeamID.TeamID
                                  select f.ID).Count();

            //Getting a list of all players
            var allPlayers = _context.Players;
            var allSeaDivTea = _context.SeasonDivisionTeams;
            var allMatches = _context.Matches;

            var homeMatches = from p in allPlayers
                              join sdt in allSeaDivTea on p.TeamID equals sdt.TeamID
                              join m in allMatches on p.ID equals m.HomePlayerID
                              where sdt.DivisionID == Convert.ToInt32(DivisionID) && sdt.SeasonID == SeasonID && p.ID == playerID
                              select new { p, m };
            var awayMatches = from p in allPlayers
                              join sdt in allSeaDivTea on p.TeamID equals sdt.TeamID
                              join m in allMatches on p.ID equals m.AwayPlayerID
                              where sdt.DivisionID == Convert.ToInt32(DivisionID) && sdt.SeasonID == SeasonID && p.ID == playerID
                              select new { p, m };

            int forScore = 0;
            int againstScore = 0;

            if(homeMatches.Count() > 0)
                foreach(var match in homeMatches)
                {
                    forScore += (int)match.m.HomePlayerScore;
                    againstScore += (int)match.m.AwayPlayerScore;
                }
            if (awayMatches.Count() > 0)
                foreach (var match in awayMatches)
                {
                    forScore += (int)match.m.AwayPlayerScore;
                    againstScore += (int)match.m.HomePlayerScore;
                }

            ps.forScore = forScore;
            ps.againstScore = againstScore;
            if (forScore + againstScore > 0)
                ps.winPerc = Math.Round(Convert.ToDecimal(forScore) / Convert.ToDecimal(forScore + againstScore) * 100, 0);
            else
                ps.winPerc = 0;
            ps.PlayerID = playerID;
            ps.PositionID = positionID;
            ps.PlayerName = playerTeamID.FirstName + " " + playerTeamID.LastName;
            decimal percTemp = Math.Round(Convert.ToDecimal(fixturesByPlayer) / Convert.ToDecimal(teamPlayedHome + teamPlayedAway) * 100);
            ps.PercPlayed = percTemp;            

            return ps;
        }

        // GET: Schedule
        public async Task<IActionResult> Schedule(int? SeasonID, string DivisionID)
        {

            var fixtures = from f in _context.Fixtures
                .Include(f => f.AwayTeam)
                .Include(f => f.CaptainApprove)
                .Include(f => f.CaptainResult)
                .Include(f => f.Division)
                .Include(f => f.HomeTeam)
                .Include(f => f.Season)
                .Include(f => f.Venue)
                           select f;

            if (SeasonID.HasValue)
            {
                fixtures = fixtures.Where(p => p.SeasonID == SeasonID);
            }
            else
            {
                SeasonID = ((from d in _context.Seasons
                             orderby d.EndDate descending
                             select d.ID).FirstOrDefault());
            }

            int userID = 0;
            int userDivision = 0;

            if (User.IsInRole("Captain") || User.IsInRole("User"))
            {
                //var captainID = from d in _context.Players
                //                where d.Email == User.Identity.Name
                //                select d;

                userID = _context.Players.FirstOrDefault(d => d.Email == User.Identity.Name).ID;
                var teamID = _context.Teams.FirstOrDefault(d => d.CaptainID == userID).ID;
                userDivision = _context.SeasonDivisionTeams.FirstOrDefault(d => d.TeamID == teamID && d.SeasonID == SeasonID).DivisionID;

                //fixtures = fixtures.Where(p => p.HomeTeamID == teamID || p.AwayTeamID == teamID);
            }


            if (DivisionID != null)
            {
                fixtures = fixtures.Where(p => p.DivisionID == Convert.ToInt32(DivisionID));
            }
            else
            {
                fixtures = fixtures.Where(p => p.DivisionID == userDivision);
            }

            PopulateDropDownListSeason();

            ViewData["UserDivision"] = userDivision;

            ViewData["DivisionID"] = from d in _context.Divisions
                                     select d;


            return View(await fixtures.ToListAsync());
        }


        public async Task<IActionResult> Violation(int? SeasonID, string DivisionID)
        {

            var fixtures = from f in _context.Fixtures
                .Include(f => f.AwayTeam)
                .Include(f => f.CaptainApprove)
                .Include(f => f.CaptainResult)
                .Include(f => f.Division)
                .Include(f => f.HomeTeam)
                .Include(f => f.Season)
                .Include(f => f.Venue)
                .Include(f => f.Matches)
                .ThenInclude(h => h.HomePlayer)
                .Include(f => f.Matches)
                .ThenInclude(a => a.AwayPlayer)
                .Include(f => f.Matches)
                .ThenInclude(p => p.Position)
                .Where(f => f.Approved == true)
                           select f;

            if (SeasonID.HasValue)
            {
                fixtures = fixtures.Where(p => p.SeasonID == SeasonID);
            }
            else
            {
                SeasonID = ((from d in _context.Seasons
                             orderby d.EndDate descending
                             select d.ID).FirstOrDefault());
            }

            int userID = 0;
            int userDivision = 0;

            if (User.IsInRole("Captain") || User.IsInRole("User"))
            {
                //var captainID = from d in _context.Players
                //                where d.Email == User.Identity.Name
                //                select d;

                userID = _context.Players.FirstOrDefault(d => d.Email == User.Identity.Name).ID;
                var teamID = _context.Teams.FirstOrDefault(d => d.CaptainID == userID).ID;
                userDivision = _context.SeasonDivisionTeams.FirstOrDefault(d => d.TeamID == teamID && d.SeasonID == SeasonID).DivisionID;

                //fixtures = fixtures.Where(p => p.HomeTeamID == teamID || p.AwayTeamID == teamID);
            }

            int pos = 5;
            if (DivisionID != null)
            {
                fixtures = fixtures.Where(p => p.DivisionID == Convert.ToInt32(DivisionID));
                if (!fixtures.Count().Equals(0))
                {
                    pos = (int)fixtures.FirstOrDefault().Division.PositionNo;
                    pos++;
                }
                    
            }
            else
            {
                fixtures = fixtures.Where(p => p.DivisionID == userDivision);
            }

            PopulateDropDownListSeason();

            ViewData["UserDivision"] = userDivision;

            ViewData["DivisionID"] = from d in _context.Divisions
                                     select d;

            

            ViewData["Positions"] = pos;

            return View(await fixtures.ToListAsync());
        }

        // GET: Fixtures/Details/5
        public async Task<IActionResult> FixtureDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures
                .Include(f => f.AwayTeam)
                .Include(f => f.CaptainApprove)
                .Include(f => f.CaptainResult)
                .Include(f => f.Division)
                .Include(f => f.HomeTeam)
                .Include(f => f.Season)
                .Include(f => f.Venue)
                .Include(f => f.Matches)
                .ThenInclude(m => m.HomePlayer)
                .Include(f => f.Matches)
                .ThenInclude(m => m.AwayPlayer)
                .Include(f => f.Matches)
                .ThenInclude(m => m.Position)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        private void PopulateDropDownListSeason()
        {
            var dQuery = from d in _context.Seasons
                         orderby d.EndDate descending
                         select d;
            ViewData["SeasonID"] = new SelectList(dQuery, "ID", "Name");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
