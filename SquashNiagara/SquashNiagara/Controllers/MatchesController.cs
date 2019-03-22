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
        public async Task<IActionResult> Create(FixtureMatchVM fixtureMatch)
        {
            //get the number of the positions for the division;
            int nPositions = _context.Divisions.FirstOrDefault(d => d.ID == fixtureMatch.Fixture.DivisionID).PositionNo;
                       
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
                    else
                        fixture.AwayTeamScore += 1;

                    //Save the fixture in the DB
                    _context.Update(fixture);
                }

                /*Inserting into TeamRanking
                 * Pending: Need to add Exception handle
                 */
                TeamRanking teamRankingHome = new TeamRanking();
                var teamRankingHomeToUpdate = _context.TeamRankings.FirstOrDefault(d => d.TeamID == fixture.HomeTeamID && d.SeasonID == fixture.SeasonID && d.DivisionID == fixture.DivisionID);

                TeamRanking teamRankingAway = new TeamRanking();
                var teamRankingAwayToUpdate = _context.TeamRankings.FirstOrDefault(d => d.TeamID == fixture.AwayTeamID && d.SeasonID == fixture.SeasonID && d.DivisionID == fixture.DivisionID);

                if (teamRankingHomeToUpdate == null)
                {
                    teamRankingHome.TeamID = fixture.HomeTeamID;
                    teamRankingHome.DivisionID = fixture.DivisionID;
                    teamRankingHome.SeasonID = fixture.SeasonID;
                    teamRankingHome.Points = (short)(fixture.HomeTeamScore + fixture.HomeTeamBonus);
                    teamRankingHome.Won = (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingHome.Lost = (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingHome.Played = 1;

                    _context.Add(teamRankingHome);
                    await _context.SaveChangesAsync();
                } else
                {
                    teamRankingHome = teamRankingHomeToUpdate;
                    teamRankingHome.Points += (short)(fixture.HomeTeamScore + fixture.HomeTeamBonus);
                    teamRankingHome.Won += (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingHome.Lost += (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingHome.Played += 1;
                    _context.Update(teamRankingHome);
                    await _context.SaveChangesAsync();
                }

                if (teamRankingAwayToUpdate == null)
                {
                    teamRankingAway.TeamID = fixture.AwayTeamID;
                    teamRankingAway.DivisionID = fixture.DivisionID;
                    teamRankingAway.SeasonID = fixture.SeasonID;
                    teamRankingAway.Points = (short)(fixture.AwayTeamScore + fixture.AwayTeamBonus);
                    teamRankingAway.Won = (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingAway.Lost = (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingAway.Played = 1;

                    _context.Add(teamRankingAway);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    teamRankingAway = teamRankingAwayToUpdate;
                    teamRankingAway.Points += (short)(fixture.AwayTeamScore + fixture.AwayTeamBonus);
                    teamRankingAway.Won += (fixture.HomeTeamScore < fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingAway.Lost += (fixture.HomeTeamScore > fixture.AwayTeamScore) ? 1 : 0;
                    teamRankingAway.Played += 1;
                    _context.Update(teamRankingAway);
                    await _context.SaveChangesAsync();
                }

                //Loop trough the matches to get the matches results and add in the DB
                for (int i = 0; i < nPositions; i++)
                {
                    //Capture the result of the match and save in the database
                    Match match = fixtureMatch.Matches[i];

                    //if (match.HomePlayerScore > match.AwayPlayerScore)
                    //    fixture.HomeTeamScore += 1;
                    //else
                    //    fixture.AwayTeamScore += 1;

                    _context.Add(match);

                    //Add in the DB the PlayerPosition for home player
                    PlayerPosition playerPositionHome = new PlayerPosition
                    {
                        PlayerID = match.HomePlayerID,
                        MatchID = match.ID,
                        //PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID
                        PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID,
                    };

                   
                    _context.Add(playerPositionHome);

                    //Add in the DB the PlayerPosition for away player
                    PlayerPosition playerPositionAway = new PlayerPosition
                    {
                        PlayerID = match.AwayPlayerID,
                        MatchID = match.ID,
                        //PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID
                        PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains((i + 1).ToString())).ID,
                    };

                    _context.Add(playerPositionAway);

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
                        playerRankingHome.Points = RankPlayer.CalcPoints(i + 1, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Home);
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
                        playerRankingHome.Points += RankPlayer.CalcPoints(i + 1, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Home);
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
                        playerRankingAway.Points = RankPlayer.CalcPoints(i + 1, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Away);
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
                        playerRankingAway.Points += RankPlayer.CalcPoints(i + 1, (short)match.HomePlayerScore, (short)match.AwayPlayerScore, ResultFor.Away);
                        playerRankingAway.Average = playerRankingAway.Points / playerRankingAway.Played;
                        _context.Update(playerRankingAway);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Fixtures");
            }
            ViewData["AwayPlayerID"] = new SelectList(_context.Players, "ID", "Email");
            ViewData["HomePlayerID"] = new SelectList(_context.Players, "ID", "Email");
            ViewData["nPositions"] = nPositions;
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
