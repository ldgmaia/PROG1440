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

namespace SquashNiagara.Controllers
{
    public class PlayersController : Controller
    {
        private readonly SquashNiagaraContext _context;

        public PlayersController(SquashNiagaraContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var squashNiagaraContext = _context.Players.Include(t => t.TeamCaptains).Include(t => t.PlayerPositions).Include(t => t.PlayerTeams);
            return View(await _context.Players.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(t => t.TeamCaptains)
                .Include(t => t.PlayerPositions)
                .Include(t => t.PlayerTeams)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            // ***********??????
            //var player = new Player(); 

           
            ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "Name");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerTeamPositionVM playerToCreate)
        {
            //if(selectedPosition != null)
            //{
            //    player.PlayerPositions = new List<PlayerPosition>();
            //    foreach(var pos in selectedPosition)
            //    {
            //        var posToAdd = new PlayerPosition { PlayerID = player.ID };
            //        player.PlayerPositions.Add(posToAdd);
            //    }
            //}
            Player player = playerToCreate.Player;
            //PlayerTeam playerTeam = new PlayerTeam();
            //playerTeam.PlayerID = player.ID;
            //playerTeam.PositionID = playerToCreate.Position;
            //playerTeam.TeamID = playerToCreate.Team;

            if (ModelState.IsValid)
            {
                
                _context.Add(player);
                //_context.Add(playerTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "Name");
            //PopulateDropDownListPosition(player);
            return View(playerToCreate);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            ViewData["TeamID"] = new SelectList(_context.Positions, "ID", "Name");
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Email,DOB,CaptainID,PositionID,TeamID")] Player player)
        {
            if (id != player.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.ID))
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
            ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            ViewData["TeamID"] = new SelectList(_context.Positions, "ID", "Name");
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(t => t.TeamCaptains)
                .Include(t => t.PlayerPositions)
                .Include(t => t.PlayerTeams)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Players/ImportUsers
        public IActionResult ImportPlayers()
        {
            return View();
        }

        // POST: Players/ImportUsers
        [HttpPost]
        public async Task<IActionResult> ImportPlayers(IFormFile theExcel)
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
                Player player = new Player
                {
                    FirstName = workSheet.Cells[row, 1].Text,
                    LastName = workSheet.Cells[row, 2].Text,
                    Email = workSheet.Cells[row, 3].Text,
                    DOB = Convert.ToDateTime(workSheet.Cells[row, 4].Value)
                };
                _context.Players.Add(player);
            };
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        //private void PopulateDropDownListPosition(Player player = null)
        //{
        //    var dQuery = from d in _context.Positions
        //                 orderby d.Name
        //                 select d;
        //    ViewData["PositionID"] = new SelectList(dQuery, "ID", "Name", player?.PlayerPositions);
        //}

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }
    }
}