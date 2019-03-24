using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SquashNiagara.Data;
using SquashNiagara.Models;
using SquashNiagara.ViewModels;
using SquashNiagaraLib;

namespace SquashNiagara.Controllers
{
    public class FixturesController : Controller
    {
        private readonly SquashNiagaraContext _context;

        public FixturesController(SquashNiagaraContext context)
        {
            _context = context;
        }

        // GET: Fixtures
        public async Task<IActionResult> Index(int? SeasonID, string DivisionID)
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
            }else
            {
                SeasonID = ((from d in _context.Seasons
                             orderby d.EndDate descending
                             select d.ID).FirstOrDefault());
            }
 
            int captainID = 0;
            int captainDivision = 0;

            if (User.IsInRole("Captain"))
            {
                //var captainID = from d in _context.Players
                //                where d.Email == User.Identity.Name
                //                select d;

                captainID = _context.Players.FirstOrDefault(d => d.Email == User.Identity.Name).ID;
                var teamID = _context.Teams.FirstOrDefault(d => d.CaptainID == captainID).ID;
                captainDivision = _context.SeasonDivisionTeams.FirstOrDefault(d => d.TeamID == teamID && d.SeasonID == SeasonID).DivisionID;

                fixtures = fixtures.Where(p => p.HomeTeamID == teamID || p.AwayTeamID == teamID);
            }
            

            if (DivisionID != null)
            {
                fixtures = fixtures.Where(p => p.DivisionID == Convert.ToInt32(DivisionID));
            }
            else
            {
                fixtures = fixtures.Where(p => p.DivisionID == captainDivision);
            }

            PopulateDropDownListSeason();

            ViewData["CaptainID"] = captainID;

            ViewData["CaptainDivision"] = captainDivision;

            ViewData["DivisionID"] = from d in _context.Divisions
                                     select d;


            return View(await fixtures.ToListAsync());
        }

        // GET: Fixtures/Details/5
        public async Task<IActionResult> Details(int? id)
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // GET: Fixtures/Create
        public IActionResult Create()
        {
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name");
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email");
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email");
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name");
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name");
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name");
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name");
            return View();
        }

        // POST: Fixtures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SeasonID,DivisionID,HomeTeamID,AwayTeamID,Date,Time,VenueID,HomeTeamScore,AwayTeamScore,HomeTeamBonus,AwayTeamBonus,Approved")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.AwayTeamID);
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainApproveID);
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainResultID);
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name", fixture.DivisionID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.HomeTeamID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name", fixture.SeasonID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", fixture.VenueID);
            return View(fixture);
        }

        // GET: Fixtures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures.FindAsync(id);
            if (fixture == null)
            {
                return NotFound();
            }
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.AwayTeamID);
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainApproveID);
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainResultID);
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name", fixture.DivisionID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.HomeTeamID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name", fixture.SeasonID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", fixture.VenueID);
            return View(fixture);
        }

        // POST: Fixtures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SeasonID,DivisionID,HomeTeamID,AwayTeamID,Date,Time,VenueID,HomeTeamScore,AwayTeamScore,HomeTeamBonus,AwayTeamBonus,Approved")] Fixture fixture)
        {
            if (id != fixture.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixtureExists(fixture.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.AwayTeamID);
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainApproveID);
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainResultID);
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name", fixture.DivisionID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.HomeTeamID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name", fixture.SeasonID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", fixture.VenueID);
            return View(fixture);
        }

        // GET: Fixtures/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // POST: Fixtures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fixture = await _context.Fixtures.FindAsync(id);
            _context.Fixtures.Remove(fixture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        // GET: Fixtures/AddResult/5
        public async Task<IActionResult> AddResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var fixture = await _context.Fixtures.FindAsync(id);
            var fixture = await _context.Fixtures
                .Include(f => f.AwayTeam)
                .Include(f => f.CaptainApprove)
                .Include(f => f.CaptainResult)
                .Include(f => f.Division)
                .Include(f => f.HomeTeam)
                .Include(f => f.Season)
                .Include(f => f.Venue)
                .FirstOrDefaultAsync(m => m.ID == id);

            var match = await _context.Matches.
                FirstOrDefaultAsync(m => m.FixtureID == id);

            if (fixture == null)
            {
                return NotFound();
            }
            ViewData["HomePlayerScore"] = match.HomePlayerScore.GetValueOrDefault();
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.AwayTeamID);
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainApproveID);
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainResultID);
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name", fixture.DivisionID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.HomeTeamID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name", fixture.SeasonID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", fixture.VenueID);
            return View(fixture);
        }

        // POST: Fixtures/AddResult/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResult(int id, [Bind("ID,SeasonID,DivisionID,HomeTeamID,AwayTeamID,Date,Time,VenueID,HomeTeamScore,AwayTeamScore,HomeTeamBonus,AwayTeamBonus")] Fixture fixture)
        {
            if (id != fixture.ID)
            {
                return NotFound();
            }

            //try
            //{
                if (ModelState.IsValid)
                {
                    _context.Update(fixture);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            //} catch (DbUpdateException err)
            //{
            //    if (err.InnerException.Message.Contains("IX_Matches_FixtureID_HomePlayerID_AwayPlayerID"))
            //    {
            //        ModelState.AddModelError("Player error", "One player cannot play two matches in the same fixture");
            //    } else
            //    {
            //        ModelState.AddModelError("", "Unable to save result. Report this error to the team");
            //    }
            //}

            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.AwayTeamID);
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainApproveID);
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainResultID);
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name", fixture.DivisionID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.HomeTeamID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name", fixture.SeasonID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", fixture.VenueID);
            return View(fixture);
        }




        // GET: Fixtures/ApproveResult
        public async Task<IActionResult> ApproveResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var fixture = await _context.Fixtures.FindAsync(id);

            var fixture = await _context.Fixtures
                .Include(f => f.AwayTeam)
                .Include(f => f.CaptainApprove)
                .Include(f => f.CaptainResult)
                .Include(f => f.Division)
                .Include(f => f.HomeTeam)
                .Include(f => f.Season)
                .Include(f => f.Venue)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (fixture == null)
            {
                return NotFound();
            }

            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.AwayTeamID);
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainApproveID);
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainResultID);
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name", fixture.DivisionID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.HomeTeamID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name", fixture.SeasonID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", fixture.VenueID);
            return View(fixture);
        }

        // POST: Fixtures/ApproveResult
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveResult(int id, [Bind("ID,SeasonID,DivisionID,HomeTeamID,AwayTeamID,Date,Time,VenueID,HomeTeamScore,AwayTeamScore,HomeTeamBonus,AwayTeamBonus,Approved,CaptainResultID,CaptainApproveID")] Fixture fixture)
        {
            if (id != fixture.ID)
            {
                return NotFound();
            }

            if (fixture.Approved == false)
            {
                return RedirectToAction(nameof(Index));
            } 

            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("Captain"))
                    {
                        fixture.CaptainApproveID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).ID;
                    }
                    _context.Update(fixture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixtureExists(fixture.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                FixtureMatchVM fixtureMatch = new FixtureMatchVM();

                fixtureMatch.Fixture = _context.Fixtures.Find(id);
               
                var matchesFromFixture = from m in _context.Matches
                    .Where(m => m.FixtureID == id)
                    .Include(m => m.Fixture)
                    .Include(m => m.Position)
                    .Include(m => m.HomePlayer)
                    .Include(m => m.AwayPlayer)
                    select m;

                fixtureMatch.Matches = matchesFromFixture.ToList();

                UpdateRankings(fixtureMatch);

                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.AwayTeamID);
            ViewData["CaptainApproveID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainApproveID);
            ViewData["CaptainResultID"] = new SelectList(_context.Players, "ID", "Email", fixture.CaptainResultID);
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "Name", fixture.DivisionID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "ID", "Name", fixture.HomeTeamID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "Name", fixture.SeasonID);
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", fixture.VenueID);
            return View(fixture);
        }

        // GET: Players/ImportUsers
        public IActionResult ImportFixtures()
        {
            return View();
        }

        // POST: Players/ImportFixtures
        [HttpPost]
        public async Task<IActionResult> ImportFixtures(IFormFile theExcel)
        {
            ExcelPackage excel;
            using (var memoryStream = new MemoryStream())
            {
                await theExcel.CopyToAsync(memoryStream);
                excel = new ExcelPackage(memoryStream);
            }
            var workSheet = excel.Workbook.Worksheets[0];
            var start = workSheet.Dimension.Start;
            var end = workSheet.Dimension.End;
            for (int row = start.Row; row <= end.Row; row++)
            {


                //var a = _context.Seasons.FirstOrDefault(d => d.Name == workSheet.Cells[row, 1].Text).ID;
                //var b = _context.Divisions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text).ID;
                //var c = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 3].Text).ID;
                //var z = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 4].Text).ID;
                //var e = Convert.ToDateTime(workSheet.Cells[row, 5].Value);
                //var f = Convert.ToDateTime(workSheet.Cells[row, 6].Text);
                //var g = _context.Venues.FirstOrDefault(d => d.Name == workSheet.Cells[row, 7].Text).ID;
                //Row by row...
                if (workSheet.Cells[row, 1].Text == "FIXTURE")
                {
                    Fixture fixture = new Fixture
                    {
                        SeasonID = _context.Seasons.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text).ID,
                        DivisionID = _context.Divisions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 3].Text).ID,
                        HomeTeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 4].Text).ID,
                        AwayTeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 5].Text).ID,
                        Date = Convert.ToDateTime(workSheet.Cells[row, 6].Value),
                        Time = Convert.ToDateTime(workSheet.Cells[row, 7].Text),
                        VenueID = _context.Venues.FirstOrDefault(d => d.Name == workSheet.Cells[row, 8].Text).ID
                    };
                    _context.Fixtures.Add(fixture);
                }
               
            };
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDownListSeason(Fixture fixture = null)
        {
            var dQuery = from d in _context.Seasons
                         orderby d.EndDate descending
                         select d;
            ViewData["SeasonID"] = new SelectList(dQuery, "ID", "Name", fixture?.Season);
        }

        private void PopulateDropDownListDivision(Fixture fixture = null)
        {
            var dQuery = from d in _context.Divisions
                         select d;
            ViewData["DivisionID"] = new SelectList(dQuery, "ID", "Name", fixture?.Division);
        }

        public void UpdateRankings(FixtureMatchVM fixtureMatch)
        {
            int nPositions = _context.Divisions.FirstOrDefault(d => d.ID == fixtureMatch.Fixture.DivisionID).PositionNo;

            Fixture fixture = fixtureMatch.Fixture;

            TeamRanking teamRankingHome = new TeamRanking();
            var teamRankingHomeToUpdate = _context.TeamRankings.FirstOrDefault(d => d.TeamID == fixture.HomeTeamID && d.SeasonID == fixture.SeasonID && d.DivisionID == fixture.DivisionID);

            TeamRanking teamRankingAway = new TeamRanking();
            var teamRankingAwayToUpdate = _context.TeamRankings.FirstOrDefault(d => d.TeamID == fixture.AwayTeamID && d.SeasonID == fixture.SeasonID && d.DivisionID == fixture.DivisionID);

            if (teamRankingHomeToUpdate == null)
            {
                teamRankingHome.TeamID = fixture.HomeTeamID;
                teamRankingHome.DivisionID = fixture.DivisionID;
                teamRankingHome.SeasonID = fixture.SeasonID;
                teamRankingHome.Points = (double)(fixture.HomeTeamScore + fixture.HomeTeamBonus);
                teamRankingHome.Won = (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                teamRankingHome.Lost = (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                teamRankingHome.Played = 1;

                _context.Add(teamRankingHome);
                _context.SaveChanges();
            }
            else
            {
                teamRankingHome = teamRankingHomeToUpdate;
                teamRankingHome.Points += (double)(fixture.HomeTeamScore + fixture.HomeTeamBonus);
                teamRankingHome.Won += (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                teamRankingHome.Lost += (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                teamRankingHome.Played += 1;
                _context.Update(teamRankingHome);
                _context.SaveChanges();
            }

            if (teamRankingAwayToUpdate == null)
            {
                teamRankingAway.TeamID = fixture.AwayTeamID;
                teamRankingAway.DivisionID = fixture.DivisionID;
                teamRankingAway.SeasonID = fixture.SeasonID;
                teamRankingAway.Points = (double)(fixture.AwayTeamScore + fixture.AwayTeamBonus);
                teamRankingAway.Won = (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                teamRankingAway.Lost = (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                teamRankingAway.Played = 1;

                _context.Add(teamRankingAway);
                _context.SaveChanges();
            }
            else
            {
                teamRankingAway = teamRankingAwayToUpdate;
                teamRankingAway.Points += (double)(fixture.AwayTeamScore + fixture.AwayTeamBonus);
                teamRankingAway.Won += (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                teamRankingAway.Lost += (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                teamRankingAway.Played += 1;
                _context.Update(teamRankingAway);
                _context.SaveChanges();
            }

            //Loop trough the matches to update the rankings
            foreach (Match match in fixtureMatch.Matches)
            {
                //Add statistics for home player
                PlayerRanking playerRankingHome = new PlayerRanking();
                var playerRankingHomeToUpdate = _context.PlayerRankings.FirstOrDefault(d => d.PlayerID == match.HomePlayerID && d.SeasonID == fixture.SeasonID && d.DivisionID == fixture.DivisionID);

                if (playerRankingHomeToUpdate == null)
                {
                    playerRankingHome.PlayerID = match.HomePlayerID;
                    playerRankingHome.SeasonID = fixture.SeasonID;
                    playerRankingHome.DivisionID = fixture.DivisionID;
                    playerRankingHome.Played = 1;
                    if (match.HomePlayerScore > match.AwayPlayerScore)
                    {
                        playerRankingHome.WonMatches = 1;
                        playerRankingHome.LostMatches = 0;
                    }
                    else
                    {
                        playerRankingHome.WonMatches = 0;
                        playerRankingHome.LostMatches = 1;
                    }

                    playerRankingHome.WonGames = (short)match.HomePlayerScore;
                    playerRankingHome.LostGames = (short)match.AwayPlayerScore;
                    playerRankingHome.Points = RankPlayer.CalcPoints(match.Position.Name, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Home);
                    playerRankingHome.Average = playerRankingHome.Points / playerRankingHome.Played;
                    _context.Add(playerRankingHome);
                }
                else
                {
                    playerRankingHome = playerRankingHomeToUpdate;
                    playerRankingHome.Played += 1;
                    if (match.HomePlayerScore > match.AwayPlayerScore)
                    {
                        playerRankingHome.WonMatches += 1;
                    }
                    else
                    {
                        playerRankingHome.LostMatches += 1;
                    }


                    playerRankingHome.WonGames += (short)match.HomePlayerScore;
                    playerRankingHome.LostGames += (short)match.AwayPlayerScore;
                    playerRankingHome.Points += RankPlayer.CalcPoints(match.Position.Name, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Home);
                    playerRankingHome.Average = playerRankingHome.Points / playerRankingHome.Played;
                    _context.Update(playerRankingHome);
                }

                //Add statistics for away player
                PlayerRanking playerRankingAway = new PlayerRanking();
                var playerRankingAwayToUpdate = _context.PlayerRankings.FirstOrDefault(d => d.PlayerID == match.AwayPlayerID && d.SeasonID == fixture.SeasonID && d.DivisionID == fixture.DivisionID);


                if (playerRankingAwayToUpdate == null)
                {
                    playerRankingAway.PlayerID = match.AwayPlayerID;
                    playerRankingAway.SeasonID = fixture.SeasonID;
                    playerRankingAway.DivisionID = fixture.SeasonID;
                    playerRankingAway.Played = 1;
                    if (match.HomePlayerScore < match.AwayPlayerScore)
                    {
                        playerRankingAway.WonMatches = 1;
                        playerRankingAway.LostMatches = 0;
                    }
                    else
                    {
                        playerRankingAway.WonMatches = 0;
                        playerRankingAway.LostMatches = 1;
                    }


                    playerRankingAway.WonGames = (short)match.AwayPlayerScore;
                    playerRankingAway.LostGames = (short)match.HomePlayerScore;
                    playerRankingAway.Points = RankPlayer.CalcPoints(match.Position.Name, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Away);
                    playerRankingAway.Average = playerRankingAway.Points / playerRankingAway.Played;
                    _context.Add(playerRankingAway);
                }
                else
                {
                    playerRankingAway = playerRankingAwayToUpdate;
                    playerRankingAway.Played += 1;
                    if (match.HomePlayerScore < match.AwayPlayerScore)
                    {
                        playerRankingAway.WonMatches += 1;
                    }
                    else
                    {
                        playerRankingAway.LostMatches += 1;
                    }

                    playerRankingAway.WonGames += (short)match.AwayPlayerScore;
                    playerRankingAway.LostGames += (short)match.HomePlayerScore;
                    playerRankingAway.Points += RankPlayer.CalcPoints(match.Position.Name, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Away);
                    playerRankingAway.Average = playerRankingAway.Points / playerRankingAway.Played;
                    _context.Update(playerRankingAway);
                }
            }
            _context.SaveChanges();
        }

        private bool FixtureExists(int id)
        {
            return _context.Fixtures.Any(e => e.ID == id);
        }
    }
}
