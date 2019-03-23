using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SquashNiagara.Data;
using SquashNiagara.Models;

namespace SquashNiagara.Controllers
{
    public class SeasonsController : Controller
    {
        private readonly SquashNiagaraContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SeasonsController(SquashNiagaraContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Seasons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Seasons.ToListAsync());
        }

        // GET: Seasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // GET: Seasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Seasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,StartDate,EndDate")] Season season)
        {
            if (ModelState.IsValid)
            {
                _context.Add(season);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(season);
        }

        // GET: Seasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }
            return View(season);
        }

        // POST: Seasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,StartDate,EndDate")] Season season)
        {
            if (id != season.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(season);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonExists(season.ID))
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
            return View(season);
        }

        // GET: Seasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Season/ImportFullSeason
        public IActionResult ImportFullSeason()
        {
            return View();
        }

        // POST: Season/ImportFullSeason
        [HttpPost]
        public async Task<IActionResult> ImportFullSeason(IFormFile theExcel)
        {
            ExcelPackage excel;
            int seasonID = 0;

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


                if (workSheet.Cells[row, 1].Text == "SEASON")
                {
                    var checkExistSeason = _context.Seasons.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text);
                    
                    if (checkExistSeason == null)
                    {
                        Season season = new Season
                        {
                            Name = workSheet.Cells[row, 2].Text,
                            StartDate = Convert.ToDateTime(workSheet.Cells[row, 4].Value),
                            EndDate = Convert.ToDateTime(workSheet.Cells[row, 6].Value)
                        };
                        _context.Seasons.Add(season);
                        _context.SaveChanges();

                        seasonID = _context.Seasons.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text).ID;
                    }
                }

                if (workSheet.Cells[row, 1].Text == "DIVISION")
                {
                    var checkExistDivison = _context.Divisions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text);

                    if (checkExistDivison == null)
                    {
                        Division division = new Division
                        {
                            Name = workSheet.Cells[row, 2].Text,
                            PositionNo = Convert.ToInt16(workSheet.Cells[row, 4].Text)
                        };
                        _context.Divisions.Add(division);
                        _context.SaveChanges();
                    }

                }

                if (workSheet.Cells[row, 1].Text == "TEAM")
                {
                    var checkExistVenue = _context.Venues.FirstOrDefault(d => d.Name == workSheet.Cells[row, 4].Text);
                    if (checkExistVenue == null)
                    {
                        Venue venue = new Venue
                        {
                            Name = workSheet.Cells[row, 4].Text
                        };
                        _context.Venues.Add(venue);
                        _context.SaveChanges();
                    }

                    int venueID = _context.Venues.FirstOrDefault(d => d.Name == workSheet.Cells[row, 4].Text).ID;
                    var checkExistTeam = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text);
                    if (checkExistTeam == null)
                    {
                        Team team = new Team
                        {
                            Name = workSheet.Cells[row, 2].Text,
                            VenueID = venueID
                        };
                        _context.Teams.Add(team);
                        _context.SaveChanges();
                    }

                    int teamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text).ID;
                    int divisionID = _context.Divisions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 6].Text).ID;
                    var checkExistSeasonDivisionTeam = _context.SeasonDivisionTeams.FirstOrDefault(d => d.SeasonID == seasonID && d.DivisionID == divisionID && d.TeamID == teamID);
                    if (checkExistSeasonDivisionTeam == null)
                    {
                        SeasonDivisionTeam seasonDivisionTeam = new SeasonDivisionTeam
                        {
                            SeasonID = seasonID,
                            DivisionID = divisionID,
                            TeamID = teamID
                        };
                        _context.SeasonDivisionTeams.Add(seasonDivisionTeam);
                        _context.SaveChanges();
                    }
                }

                if (workSheet.Cells[row, 1].Text == "CAPTAIN")
                {
                    var checkExistCaptain = _context.Players.FirstOrDefault(d => d.Email == workSheet.Cells[row, 5].Text);
                    if (checkExistCaptain == null)
                    {
                        Player captain = new Player
                        {
                            FirstName = workSheet.Cells[row, 2].Text,
                            LastName = workSheet.Cells[row, 3].Text,
                            DOB = Convert.ToDateTime(workSheet.Cells[row, 4].Value),
                            Email = workSheet.Cells[row, 5].Text,
                            //PositionID = _context.Positions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 6].Text).ID,
                            PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains(workSheet.Cells[row, 6].Text)).ID,
                            TeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 8].Text).ID
                        };
                        IdentityUser newUser = new IdentityUser
                        {
                            UserName = captain.Email,
                            Email = captain.Email
                        };

                        IdentityResult result = _userManager.CreateAsync(newUser, "password").Result;

                        if (result.Succeeded)
                            _userManager.AddToRoleAsync(newUser, "User").Wait();

                        //var user = await _userManager.FindByEmailAsync(captain.Email);

                        var newRoleToCaptain = _userManager.AddToRoleAsync(newUser, "Captain");

                        _context.Players.Add(captain);
                        _context.SaveChanges();

                        if (workSheet.Cells[row, 8].Text == "FALSE")
                        {
                            captain.IsEnabled = false;
                            await _userManager.SetLockoutEndDateAsync(newUser, DateTimeOffset.MaxValue);
                        }

                        Team teamToUpdate = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 8].Text);
                        int captainID = _context.Players.FirstOrDefault(d => d.Email == captain.Email).ID;
                        teamToUpdate.CaptainID = captainID;
                        _context.Teams.Update(teamToUpdate);
                    }
                    
                }

                if (workSheet.Cells[row, 1].Text == "PLAYER")
                {
                    var checkExistPlayer = _context.Players.FirstOrDefault(d => d.Email == workSheet.Cells[row, 5].Text);
                    if (checkExistPlayer == null)
                    {
                        Player player = new Player
                        {
                            FirstName = workSheet.Cells[row, 2].Text,
                            LastName = workSheet.Cells[row, 3].Text,
                            DOB = Convert.ToDateTime(workSheet.Cells[row, 4].Value),
                            Email = workSheet.Cells[row, 5].Text,
                            PositionID = _context.Positions.FirstOrDefault(d => d.Name.Contains(workSheet.Cells[row, 6].Text)).ID,
                            TeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 8].Text).ID
                        };
                        IdentityUser newUser = new IdentityUser
                        {
                            UserName = player.Email,
                            Email = player.Email
                        };

                        IdentityResult result = _userManager.CreateAsync(newUser, "password").Result;

                        if (result.Succeeded)
                            _userManager.AddToRoleAsync(newUser, "User").Wait();

                        if (workSheet.Cells[row, 8].Text == "FALSE")
                        {
                            player.IsEnabled = false;
                            await _userManager.SetLockoutEndDateAsync(newUser, DateTimeOffset.MaxValue);
                        }

                        
                        _context.Players.Add(player);
                        _context.SaveChanges();
                    }

                }

                //Fixture fixture = new Fixture
                //{
                //    SeasonID = _context.Seasons.FirstOrDefault(d => d.Name == workSheet.Cells[row, 1].Text).ID,
                //    DivisionID = _context.Divisions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 2].Text).ID,
                //    HomeTeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 3].Text).ID,
                //    AwayTeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 4].Text).ID,
                //    Date = Convert.ToDateTime(workSheet.Cells[row, 5].Value),
                //    Time = Convert.ToDateTime(workSheet.Cells[row, 6].Text),
                //    VenueID = _context.Venues.FirstOrDefault(d => d.Name == workSheet.Cells[row, 7].Text).ID
                //};
                //_context.Fixtures.Add(fixture);
            };
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.ID == id);
        }
    }
}
