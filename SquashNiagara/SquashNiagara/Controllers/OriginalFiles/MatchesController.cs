//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using SquashNiagara.Data;
//using SquashNiagara.Models;

//namespace SquashNiagara.Controllers
//{
//    public class MatchesController : Controller
//    {
//        private readonly SquashNiagaraContext _context;

//        public MatchesController(SquashNiagaraContext context)
//        {
//            _context = context;
//        }

//        // GET: Matches
//        public async Task<IActionResult> Index()
//        {
//            var squashNiagaraContext = _context.Matches.Include(m => m.AwayPlayer).Include(m => m.Fixture).Include(m => m.HomePlayer);
//            return View(await squashNiagaraContext.ToListAsync());
//        }

//        // GET: Matches/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var match = await _context.Matches
//                .Include(m => m.AwayPlayer)
//                .Include(m => m.Fixture)
//                .Include(m => m.HomePlayer)
//                .FirstOrDefaultAsync(m => m.ID == id);
//            if (match == null)
//            {
//                return NotFound();
//            }

//            return View(match);
//        }

//        // GET: Matches/Create
//        public IActionResult Create()
//        {
//            ViewData["AwayPlayerID"] = new SelectList(_context.Players, "ID", "Email");
//            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID");
//            ViewData["HomePlayerID"] = new SelectList(_context.Players, "ID", "Email");
//            return View();
//        }

//        // POST: Matches/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("ID,FixtureID,HomePlayerID,AwayPlayerID,HomePlayerScore,AwayPlayerScore")] Match match)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(match);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AwayPlayerID"] = new SelectList(_context.Players, "ID", "Email", match.AwayPlayerID);
//            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
//            ViewData["HomePlayerID"] = new SelectList(_context.Players, "ID", "Email", match.HomePlayerID);
//            return View(match);
//        }

//        // GET: Matches/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var match = await _context.Matches.FindAsync(id);
//            if (match == null)
//            {
//                return NotFound();
//            }
//            ViewData["AwayPlayerID"] = new SelectList(_context.Players, "ID", "Email", match.AwayPlayerID);
//            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
//            ViewData["HomePlayerID"] = new SelectList(_context.Players, "ID", "Email", match.HomePlayerID);
//            return View(match);
//        }

//        // POST: Matches/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("ID,FixtureID,HomePlayerID,AwayPlayerID,HomePlayerScore,AwayPlayerScore")] Match match)
//        {
//            if (id != match.ID)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(match);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!MatchExists(match.ID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AwayPlayerID"] = new SelectList(_context.Players, "ID", "Email", match.AwayPlayerID);
//            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
//            ViewData["HomePlayerID"] = new SelectList(_context.Players, "ID", "Email", match.HomePlayerID);
//            return View(match);
//        }

//        // GET: Matches/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var match = await _context.Matches
//                .Include(m => m.AwayPlayer)
//                .Include(m => m.Fixture)
//                .Include(m => m.HomePlayer)
//                .FirstOrDefaultAsync(m => m.ID == id);
//            if (match == null)
//            {
//                return NotFound();
//            }

//            return View(match);
//        }

//        // POST: Matches/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var match = await _context.Matches.FindAsync(id);
//            _context.Matches.Remove(match);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool MatchExists(int id)
//        {
//            return _context.Matches.Any(e => e.ID == id);
//        }
//    }
//}
