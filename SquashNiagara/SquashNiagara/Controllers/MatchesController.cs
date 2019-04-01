using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SquashNiagara.Data;
using SquashNiagara.Models;
using SquashNiagara.ViewModels;
using SquashNiagaraLib;

namespace SquashNiagara.Controllers
{
    public class MatchesController : Controller
    {
        private readonly SquashNiagaraContext _context;

        public MatchesController(SquashNiagaraContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var squashNiagaraContext = _context.Matches.Include(m => m.AwayPlayer).Include(m => m.Fixture).Include(m => m.HomePlayer);
            return View(await squashNiagaraContext.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.AwayPlayer)
                .Include(m => m.Fixture)
                .Include(m => m.HomePlayer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public async Task<IActionResult> Create(int? id)
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

            //int nPositions = 4;
            int nPositions = _context.Divisions.FirstOrDefault(d => d.ID == fixture.DivisionID).PositionNo;

            if (fixture == null)
            {
                return NotFound();
            }

            FixtureMatchVM fixtureMatch = new FixtureMatchVM();
            fixtureMatch.Fixture = fixture;

            for (int i = 0; i < nPositions; i++)
            {
                Match match = new Match();
                fixtureMatch.Matches.Add(match);
            }

            //.FirstOrDefaultAsync(m => m.ID == fixture.DivisionID);

            var dQueryHome = from d in _context.Players
                         orderby d.FirstName, d.LastName
                         where d.TeamID == fixture.HomeTeamID
                         select d;

            var dQueryAway = from d in _context.Players
                             orderby d.FirstName, d.LastName
                             where d.TeamID == fixture.AwayTeamID
                             select d;

            var homeTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.HomeTeamID);
            var awayTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.AwayTeamID);

            ViewData["HomeTeam"] = homeTeam;
            ViewData["AwayTeam"] = awayTeam;

            ViewData["AwayPlayerID"] = new SelectList(dQueryAway, "ID", "FullName");
            ViewData["HomePlayerID"] = new SelectList(dQueryHome, "ID", "FullName");
            ViewData["nPositions"] = nPositions;
            return View(fixtureMatch);
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FixtureMatchVM fixtureMatch, string Bonus)
        {
            //get the number of the positions for the division;
            int nPositions = _context.Divisions.FirstOrDefault(d => d.ID == fixtureMatch.Fixture.DivisionID).PositionNo;
            if (Bonus == "Home")
            {
                fixtureMatch.Fixture.HomeTeamBonus = 1d;
                fixtureMatch.Fixture.AwayTeamBonus = 0d;
            } else if (Bonus == "Away")
            {
                fixtureMatch.Fixture.HomeTeamBonus = 0d;
                fixtureMatch.Fixture.AwayTeamBonus = 1d;
            }
            else if (Bonus == "Tie")
            {
                fixtureMatch.Fixture.HomeTeamBonus = 0.5d;
                fixtureMatch.Fixture.AwayTeamBonus = 0.5d;
            }

            try
            {
                if (ModelState.IsValid)
                {
                    //Get the fixture from model FixtureMatch
                    Fixture fixture = fixtureMatch.Fixture;
                    fixture.HomeTeamScore = 0;
                    fixture.AwayTeamScore = 0;
                    for (int n = 0; n < nPositions; n++)
                    {
                        if (fixtureMatch.Matches[n].HomePlayerScore > fixtureMatch.Matches[n].AwayPlayerScore)
                            fixture.HomeTeamScore += 1;
                        else if (fixtureMatch.Matches[n].HomePlayerScore < fixtureMatch.Matches[n].AwayPlayerScore)
                            fixture.AwayTeamScore += 1;
                        else
                        {
                            ModelState.AddModelError("Score error", "No results can be blank");
                            return View(fixtureMatch);
                        }

                        //Save the fixture in the DB
                        if (User.IsInRole("Captain"))
                        {
                            fixture.CaptainResultID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).ID;
                        }
                        _context.Update(fixture);
                    }

                    //Loop trough the matches to get the matches results and add in the DB
                    for (int i = 0; i < nPositions; i++)
                    {
                        //Capture the result of the match and save in the database
                        Match match = fixtureMatch.Matches[i];
                        match.PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID;

                        _context.Add(match);

                        //Add in the DB the PlayerPosition for home player
                        PlayerPosition playerPositionHome = new PlayerPosition
                        {
                            PlayerID = match.HomePlayerID,
                            MatchID = match.ID,
                            PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID,
                        };

                        _context.Add(playerPositionHome);

                        //Add in the DB the PlayerPosition for away player
                        PlayerPosition playerPositionAway = new PlayerPosition
                        {
                            PlayerID = match.AwayPlayerID,
                            MatchID = match.ID,
                            PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID,
                        };
                        _context.Add(playerPositionAway);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Fixtures");
                }
            }
            catch (DbUpdateException err)
            {
                if (err.InnerException.Message.Contains("IX_Matches_FixtureID_HomePlayerID_AwayPlayerID"))
                {
                    ModelState.AddModelError("Player error", "One player cannot play two matches in the same fixture");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save result. Report this error to the team");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save result. Report this error to the team");
            }
            finally
            {
                var dQueryHome = from d in _context.Players
                                 orderby d.FirstName, d.LastName
                                 where d.TeamID == fixtureMatch.Fixture.HomeTeamID
                                 select d;

                var dQueryAway = from d in _context.Players
                                 orderby d.FirstName, d.LastName
                                 where d.TeamID == fixtureMatch.Fixture.AwayTeamID
                                 select d;

                ViewData["AwayPlayerID"] = new SelectList(dQueryAway, "ID", "FullName");
                ViewData["HomePlayerID"] = new SelectList(dQueryHome, "ID", "FullName");
                ViewData["nPositions"] = nPositions;
            }
            
            return View(fixtureMatch);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var matches = _context.Matches
                .Include(m => m.Fixture)
                .Include(m => m.HomePlayer)
                .Include(m => m.AwayPlayer)
                .Include(m => m.Position)
                .Where(m => m.FixtureID == id)
                .OrderBy(m => m.Position.Name);


            //int nPositions = 4;
            int nPositions = _context.Divisions.FirstOrDefault(d => d.ID == fixture.DivisionID).PositionNo;

            if (fixture == null)
            {
                return NotFound();
            }

            FixtureMatchVM fixtureMatch = new FixtureMatchVM();
            fixtureMatch.Fixture = fixture;
            foreach (Match match in matches)
                fixtureMatch.Matches.Add(match);


            //.FirstOrDefaultAsync(m => m.ID == fixture.DivisionID);

            var dQueryHome = from d in _context.Players
                             orderby d.FirstName, d.LastName
                             where d.TeamID == fixture.HomeTeamID
                             select d;

            var dQueryAway = from d in _context.Players
                             orderby d.FirstName, d.LastName
                             where d.TeamID == fixture.AwayTeamID
                             select d;

            int pos1ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 1")).ID;
            int pos2ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 2")).ID;
            int pos3ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 3")).ID;
            int pos4ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 4")).ID;
            int pos5ID = 0;
            if (nPositions == 5)
                pos5ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 5")).ID;

            int hpPos1 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos1ID).HomePlayerID;
            int apPos1 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos1ID).AwayPlayerID;

            int hpPos2 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos2ID).HomePlayerID;
            int apPos2 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos2ID).AwayPlayerID;

            int hpPos3 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos3ID).HomePlayerID;
            int apPos3 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos3ID).AwayPlayerID;

            int hpPos4 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos4ID).HomePlayerID;
            int apPos4 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos4ID).AwayPlayerID;

            int hpPos5 = 0;
            int apPos5 = 0;

            if (nPositions == 5)
            {
                hpPos5 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos5ID).HomePlayerID;
                apPos5 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos5ID).AwayPlayerID;
            }

            ViewData["HomePlayerIDPos1"] = new SelectList(dQueryHome, "ID", "FullName", hpPos1);
            ViewData["HomePlayerIDPos2"] = new SelectList(dQueryHome, "ID", "FullName", hpPos2);
            ViewData["HomePlayerIDPos3"] = new SelectList(dQueryHome, "ID", "FullName", hpPos3);
            ViewData["HomePlayerIDPos4"] = new SelectList(dQueryHome, "ID", "FullName", hpPos4);

            if (nPositions == 5)
                ViewData["HomePlayerIDPos5"] = new SelectList(dQueryHome, "ID", "FullName", hpPos5);

            ViewData["AwayPlayerIDPos1"] = new SelectList(dQueryAway, "ID", "FullName", apPos1);
            ViewData["AwayPlayerIDPos2"] = new SelectList(dQueryAway, "ID", "FullName", apPos2);
            ViewData["AwayPlayerIDPos3"] = new SelectList(dQueryAway, "ID", "FullName", apPos3);
            ViewData["AwayPlayerIDPos4"] = new SelectList(dQueryAway, "ID", "FullName", apPos4);

            if (nPositions == 5)
                ViewData["AwayPlayerIDPos5"] = new SelectList(dQueryAway, "ID", "FullName", apPos5);

            ViewData["nPositions"] = nPositions;
            return View(fixtureMatch);

        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FixtureMatchVM fixtureMatch, string Bonus)
        {
            //get the number of the positions for the division;
            int nPositions = _context.Divisions.FirstOrDefault(d => d.ID == fixtureMatch.Fixture.DivisionID).PositionNo;
            if (Bonus == "Home")
            {
                fixtureMatch.Fixture.HomeTeamBonus = 1d;
                fixtureMatch.Fixture.AwayTeamBonus = 0d;
            }
            else if (Bonus == "Away")
            {
                fixtureMatch.Fixture.HomeTeamBonus = 0d;
                fixtureMatch.Fixture.AwayTeamBonus = 1d;
            }
            else if (Bonus == "Tie")
            {
                fixtureMatch.Fixture.HomeTeamBonus = 0.5d;
                fixtureMatch.Fixture.AwayTeamBonus = 0.5d;
            }
            Fixture fixture = fixtureMatch.Fixture;

            try
            {
                if (ModelState.IsValid)
                {
                    //Get the fixture from model FixtureMatch
                    //Fixture fixture = fixtureMatch.Fixture;
                    fixture.HomeTeamScore = 0;
                    fixture.AwayTeamScore = 0;
                    for (int n = 0; n < nPositions; n++)
                    {
                        if (fixtureMatch.Matches[n].HomePlayerScore > fixtureMatch.Matches[n].AwayPlayerScore)
                            fixture.HomeTeamScore += 1;
                        else if (fixtureMatch.Matches[n].HomePlayerScore < fixtureMatch.Matches[n].AwayPlayerScore)
                            fixture.AwayTeamScore += 1;
                        else
                        {
                            ModelState.AddModelError("Score error", "No results can be blank");
                            return View(fixtureMatch);
                        }

                        //Save the fixture in the DB
                        if (User.IsInRole("Captain"))
                        {
                            fixture.CaptainResultID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).ID;
                        }
                        _context.Update(fixture);
                    }

                    //Loop trough the matches to get the matches results and add in the DB
                    for (int i = 0; i < nPositions; i++)
                    {
                        //Capture the result of the match and save in the database
                        Match match = fixtureMatch.Matches[i];
                        match.PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID;

                        _context.Update(match);

                        //Add in the DB the PlayerPosition for home player
                        PlayerPosition playerPositionHome = new PlayerPosition
                        {
                            PlayerID = match.HomePlayerID,
                            MatchID = match.ID,
                            PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID,
                        };

                        _context.Update(playerPositionHome);

                        //Add in the DB the PlayerPosition for away player
                        PlayerPosition playerPositionAway = new PlayerPosition
                        {
                            PlayerID = match.AwayPlayerID,
                            MatchID = match.ID,
                            PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID,
                        };
                        _context.Update(playerPositionAway);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Fixtures");
                }
            }
            catch (DbUpdateException err)
            {
                if (err.InnerException.Message.Contains("IX_Matches_FixtureID_HomePlayerID_AwayPlayerID"))
                {
                    ModelState.AddModelError("Player error", "One player cannot play two matches in the same fixture");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save result. Report this error to the team");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save result. Report this error to the team");
            }
            finally
            {
                var dQueryHome = from d in _context.Players
                                 orderby d.FirstName, d.LastName
                                 where d.TeamID == fixtureMatch.Fixture.HomeTeamID
                                 select d;

                var dQueryAway = from d in _context.Players
                                 orderby d.FirstName, d.LastName
                                 where d.TeamID == fixtureMatch.Fixture.AwayTeamID
                                 select d;

                //ViewData["AwayPlayerID"] = new SelectList(dQueryAway, "ID", "FullName");
                //ViewData["HomePlayerID"] = new SelectList(dQueryHome, "ID", "FullName");
                //ViewData["nPositions"] = nPositions;

                int pos1ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 1")).ID;
                int pos2ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 2")).ID;
                int pos3ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 3")).ID;
                int pos4ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 4")).ID;
                int pos5ID = 0;
                if (nPositions == 5)
                    pos5ID = _context.Positions.FirstOrDefault(d => d.Name.Contains("Position 5")).ID;

                int hpPos1 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos1ID).HomePlayerID;
                int apPos1 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos1ID).AwayPlayerID;

                int hpPos2 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos2ID).HomePlayerID;
                int apPos2 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos2ID).AwayPlayerID;

                int hpPos3 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos3ID).HomePlayerID;
                int apPos3 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos3ID).AwayPlayerID;

                int hpPos4 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos4ID).HomePlayerID;
                int apPos4 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos4ID).AwayPlayerID;

                int hpPos5 = 0;
                int apPos5 = 0;

                if (nPositions == 5)
                {
                    hpPos5 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos5ID).HomePlayerID;
                    apPos5 = _context.Matches.FirstOrDefault(d => d.FixtureID == fixture.ID && d.PositionID == pos5ID).AwayPlayerID;
                }

                ViewData["HomePlayerIDPos1"] = new SelectList(dQueryHome, "ID", "FullName", hpPos1);
                ViewData["HomePlayerIDPos2"] = new SelectList(dQueryHome, "ID", "FullName", hpPos2);
                ViewData["HomePlayerIDPos3"] = new SelectList(dQueryHome, "ID", "FullName", hpPos3);
                ViewData["HomePlayerIDPos4"] = new SelectList(dQueryHome, "ID", "FullName", hpPos4);

                if (nPositions == 5)
                    ViewData["HomePlayerIDPos5"] = new SelectList(dQueryHome, "ID", "FullName", hpPos5);

                ViewData["AwayPlayerIDPos1"] = new SelectList(dQueryAway, "ID", "FullName", apPos1);
                ViewData["AwayPlayerIDPos2"] = new SelectList(dQueryAway, "ID", "FullName", apPos2);
                ViewData["AwayPlayerIDPos3"] = new SelectList(dQueryAway, "ID", "FullName", apPos3);
                ViewData["AwayPlayerIDPos4"] = new SelectList(dQueryAway, "ID", "FullName", apPos4);

                if (nPositions == 5)
                    ViewData["AwayPlayerIDPos5"] = new SelectList(dQueryAway, "ID", "FullName", apPos5);

                ViewData["nPositions"] = nPositions;
            }

            return View(fixtureMatch);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.AwayPlayer)
                .Include(m => m.Fixture)
                .Include(m => m.HomePlayer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.ID == id);
        }
    }
}
