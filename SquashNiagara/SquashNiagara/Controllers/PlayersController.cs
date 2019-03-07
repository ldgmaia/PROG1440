using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            //var context = from p in _context.Players
            //              .Include(p => p.Team)
            //              .Include(p => p.Positions)
            //              .ThenInclude(t => t.Position)
            //              select p;

            var context = from p in _context.Players
                          .Include(p => p.Team)
                          .Include(p => p.Position)
                          select p;

            return View(await context.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(t => t.Team)
                .Include(t => t.Position)
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
            var player = new Player();
            //player.PlayerPositions = new List<PlayerPosition>();
            //PopulateAssignedPositionData(player);

            //ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            //ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            PopulateDropDownListTeam();
            PopulateDropDownListPosition();
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Email,DOB")] Player player)
        {
            try
            {
                //Add the selected conditions
                //if (selectedPosition != null)
                //{
                //    //player.PlayerPositions = new List<PlayerPosition>();
                //    //foreach (var pos in selectedPosition)
                //    //{
                //    //    var posToAdd = new PlayerPosition { PlayerID = player.ID, PositionID = int.Parse(pos) };
                //    //    player.PlayerPositions.Add(posToAdd);
                //    //}
                //}


                if (ModelState.IsValid)
                {
                    _context.Add(player);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DataException dex)
            {

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            catch (Exception e)
            {

                ModelState.AddModelError("", "Unable to spit");
            }

            //PopulateAssignedPositionData(player);         

            //ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            //ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            //ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "Name");
            PopulateDropDownListTeam();
            PopulateDropDownListPosition();
            //PopulateDropDownListPosition(player);
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //var player = await _context.Players
            //    .Include(p => p.PlayerPositions).ThenInclude(p => p.Position)
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(p => p.ID == id);

            //if (player == null)
            //{
            //    return NotFound();
            //}

            //ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            //ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            //PopulateDropDownListTeam(player);
            //return View(player);
            return View();
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id /*string[] selectedPositions*/)
        {
            //Go get the player to update
            //var playerToUpdate = await _context.Players
            //    .Include(p => p.Position).ThenInclude(p => p.Team)
            //    .SingleOrDefaultAsync(p => p.ID == id);

            //Check that you got it or exit with a not found error
            //if (playerToUpdate == null)
            //{
            //    return NotFound();
            //}

            //Update the medical history
            //UpdatePlayerPosition(selectedPositions, playerToUpdate);

            //Try updating it with the values posted
            //if (await TryUpdateModelAsync<Player>(playerToUpdate, "",
            //    p => p.FirstName, p => p.LastName, p => p.Email, p => p.DOB))
            //{
            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    catch (RetryLimitExceededException /* dex */)
            //    {
            //        ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!PlayerExists(playerToUpdate.ID))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    catch (DbUpdateException dex)
            //    {
            //        if (dex.InnerException.Message.Contains("IX_Players_OHIP"))
            //        {
            //            ModelState.AddModelError("OHIP", "Unable to save changes. Remember, you cannot have duplicate OHIP numbers.");
            //        }
            //        else
            //        {
            //            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            //        }
            //    }
            //}
            ////Validaiton Error so give the user another chance.
            //PopulateAssignedPositionData(playerToUpdate);
            return View();
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(t => t.Team)
                .Include(t => t.Position)
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

        //private void PopulateAssignedPositionData(Player player)
        //{
        //    var allPositions = _context.Positions;
        //    var pPositions = new HashSet<int>(player.PlayerPositions.Select(b => b.PositionID));
        //    var viewModel = new List<PlayerPositionVM>();
        //    foreach (var pos in allPositions)
        //    {
        //        viewModel.Add(new PlayerPositionVM
        //        {
        //            PositionID = pos.ID,
        //            PositionName = pos.Name,
        //            Assigned = pPositions.Contains(pos.ID)
        //        });
        //    }
        //    ViewData["Positions"] = viewModel;
        //}

        //private void UpdatePlayerPosition(string[] selectedPositions, Player playerToUpdate)
        //{
        //    if (selectedPositions == null)
        //    {
        //        playerToUpdate.PlayerPositions = new List<PlayerPosition>();
        //        return;
        //    }

        //    var selectedPositionHS = new HashSet<string>(selectedPositions);
        //    var playerPos = new HashSet<int>
        //        (playerToUpdate.PlayerPositions.Select(c => c.PositionID));//IDs of the currently selected
        //    foreach (var pos in _context.Positions)
        //    {
        //        if (selectedPositionHS.Contains(pos.ID.ToString()))
        //        {
        //            if (!playerPos.Contains(pos.ID))
        //            {
        //                playerToUpdate.PlayerPositions.Add(new PlayerPosition { PlayerID = playerToUpdate.ID, PositionID = pos.ID });
        //            }
        //        }
        //        else
        //        {
        //            if (playerPos.Contains(pos.ID))
        //            {
        //                PlayerPosition positionToRemove = playerToUpdate.PlayerPositions.SingleOrDefault(c => c.PositionID == pos.ID);
        //                _context.Remove(positionToRemove);
        //            }
        //        }
        //    }
        //}

        private void PopulateDropDownListTeam(Player player = null)
        {
            var dQuery = from d in _context.Teams
                         orderby d.Name
                         select d;
            ViewData["TeamID"] = new SelectList(dQuery, "ID", "Name", player?.Team);
        }

        private void PopulateDropDownListPosition(Player player = null)
        {
            var dQuery = from d in _context.Positions
                         orderby d.Name
                         select d;
            ViewData["PositionID"] = new SelectList(dQuery, "ID", "Name", player?.Position);
        }


        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }
    }
}