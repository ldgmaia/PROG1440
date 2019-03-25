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


            //.FirstOrDefaultAsync(m => m.ID == fixture.DivisionID);

            var dQueryHome = from d in _context.Players
                         orderby d.FirstName, d.LastName
                         where d.TeamID == fixture.HomeTeamID
                         select d;

            var dQueryAway = from d in _context.Players
                             orderby d.FirstName, d.LastName
                             where d.TeamID == fixture.AwayTeamID
                             select d;

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

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["AwayPlayerID"] = new SelectList(_context.Players, "ID", "Email", match.AwayPlayerID);
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
            ViewData["HomePlayerID"] = new SelectList(_context.Players, "ID", "Email", match.HomePlayerID);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FixtureID,HomePlayerID,AwayPlayerID,HomePlayerScore,AwayPlayerScore")] Match match)
        {
            if (id != match.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.ID))
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
            ViewData["AwayPlayerID"] = new SelectList(_context.Players, "ID", "Email", match.AwayPlayerID);
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
            ViewData["HomePlayerID"] = new SelectList(_context.Players, "ID", "Email", match.HomePlayerID);
            return View(match);
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
