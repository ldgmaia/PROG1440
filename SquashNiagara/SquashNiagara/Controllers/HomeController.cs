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

        public IActionResult Standings()
        {
            ViewBag.error = "";
            try
            {
                //Getting the number of positions by division - I'm using hardcoded Division 1
                var nPositions = _context.Divisions.Include(p => p.PositionNo);
                var pos = from p in nPositions
                          where p.ID == 1
                          select p.PositionNo;
                int currentPosition = (int)pos.ToList()[0];

                //Getting the list o player that played in given position
                var playerPositions = _context.PlayerPositions.Include(pp => pp.PlayerID);
                var matches = _context.Matches.Include(m => m.ID);
                var fixtures = _context.Fixtures.Include(f => f.ID);
                var players = _context.Players.Include(p => p.ID);

                var playersByPosition =
                   (from pp in playerPositions
                    join m in matches on pp.MatchID equals m.ID
                    join f in fixtures on m.FixtureID equals f.ID
                    join p in players on pp.PlayerID equals p.ID
                    where f.SeasonID == 1 && f.DivisionID == 1 && f.HomeTeamScore != null
                    orderby pp.PositionID
                    select new PositionalStandings { PlayerID = p.ID, PlayerName = p.FirstName + " " + p.LastName, PositionID = pp.PositionID }).Distinct();

                List<PositionalStandings> newPS = new List<PositionalStandings>();

                foreach (var r in playersByPosition.ToList())
                {
                    PositionalStandings temp = new PositionalStandings();
                    temp.PlayerID = r.PlayerID;
                    temp.PlayerName = r.PlayerName;
                    temp.PositionID = r.PositionID;

                    //getting the team of the player
                    var playerTeam = _context.Players.Include(p => p.TeamID);
                    var pt = from p in playerTeam
                             where p.ID == r.PlayerID
                             select p.TeamID;
                    int teamID = (int)pt.ToList()[0];

                    //getting the number of fixtures by team
                    var nFixturesByTeam = _context.Fixtures
                        .Include(f => f.ID)
                        .Include(f => f.SeasonID)
                        .Include(f => f.DivisionID)
                        .Include(f => f.HomeTeamID)
                        .Include(f => f.AwayTeamID)
                        .Include(f => f.HomeTeamScore);
                    var fixturesByTeam = (from f in nFixturesByTeam
                                          where f.SeasonID == 1
                                          && f.DivisionID == 1
                                          && (f.HomeTeamID == teamID || f.AwayTeamID == teamID)
                                          && f.HomeTeamScore != null
                                          select f.ID).Count();

                    //getting the number of fixtures by player
                    var nFixturesByPlayer = _context.Fixtures.Include(f => f.ID);
                    var nMatches = _context.Matches
                        .Include(m => m.ID)
                        .Include(m => m.FixtureID)
                        .Include(m => m.HomePlayerID)
                        .Include(m => m.AwayPlayerID)
                        ;
                    var fixturesByPlayer = (from f in nFixturesByPlayer
                                            join m in nMatches on f.ID equals m.FixtureID
                                            where f.SeasonID == 1
                                            && f.DivisionID == 1
                                            && (m.HomePlayerID == r.PlayerID || m.AwayPlayerID == r.PlayerID)
                                            select f.ID).Count();

                    decimal pTemp = Convert.ToDecimal(fixturesByPlayer) / Convert.ToDecimal(fixturesByTeam);
                    temp.PercPlayed = Math.Round(pTemp * 100, 0);
                    newPS.Add(temp);
                    //Debug.WriteLine("-------" + r.PlayerName + ", " + r.PositionID + ", " + fixturesByPlayer + ", " + fixturesByTeam);
                }

                ViewBag.positions = pos.ToList();
                ViewBag.playersByPosition = newPS;
            }catch(ArgumentOutOfRangeException err)
            {
                ViewBag.error = "No matches have been played. Add fixture results first.";
            } catch(Exception err)
            {
                ViewBag.error = "Some problem happend that need to be analised. Report it to the team";
            }
            return View();
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
