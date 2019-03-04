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
        public async Task<IActionResult> Index()
        {
            var squashNiagaraContext = _context.Fixtures.Include(f => f.AwayTeam).Include(f => f.CaptainApprove).Include(f => f.CaptainResult).Include(f => f.Division).Include(f => f.HomeTeam).Include(f => f.Season).Include(f => f.Venue);
            return View(await squashNiagaraContext.ToListAsync());
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
        public async Task<IActionResult> ApproveResult(int id, [Bind("ID,SeasonID,DivisionID,HomeTeamID,AwayTeamID,Date,Time,VenueID,HomeTeamScore,AwayTeamScore,HomeTeamBonus,AwayTeamBonus,Approved")] Fixture fixture)
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
                // Row by row...
                Fixture fixture = new Fixture
                {
                    SeasonID = 1
                };
                _context.Fixtures.Add(fixture);
            };
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool FixtureExists(int id)
        {
            return _context.Fixtures.Any(e => e.ID == id);
        }
    }
}
