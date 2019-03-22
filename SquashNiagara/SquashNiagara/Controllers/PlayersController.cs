using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public PlayersController(SquashNiagaraContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {   
            if (User.IsInRole("Captain") || User.IsInRole("User"))
            {                
                var player = await _context.Players
                .Include(t => t.Team)
                .Include(t => t.Position)
                .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);
                return RedirectToAction("Details", new { id = player.ID });

            }
            var playersList = _context.Players.Include(t => t.Position).Include(t => t.Team);
            return View(await playersList.ToListAsync());
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
            PopulateDropDownListTeam();
            PopulateDropDownListPosition();
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Email,DOB,TeamID,IsEnabled,PositionID")] Player player, string chkRemoveImage, IFormFile thePicture)
        {
            
            //if (player.PositionID == 0)
            //{
            //player.PositionID == null;
            //}

            if (player == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (thePicture != null)
                    {
                        string mimeType = thePicture.ContentType;
                        long fileLength = thePicture.Length;
                        if (!(mimeType == "" || fileLength == 0))
                        {
                            if (mimeType.Contains("image"))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    await thePicture.CopyToAsync(memoryStream);
                                    player.imageContent = memoryStream.ToArray();
                                }
                                player.imageMimeType = mimeType;
                                player.imageFileName = thePicture.FileName;
                            }
                        }
                    }

                    //create user Identity for player - BEGIN
                    IdentityUser newUser = new IdentityUser
                    {
                        UserName = player.Email,
                        Email = player.Email
                    };                    

                    IdentityResult result = _userManager.CreateAsync(newUser, "password").Result;

                    if (result.Succeeded)
                        _userManager.AddToRoleAsync(newUser, "User").Wait();

                    if (!player.IsEnabled)
                        await _userManager.SetLockoutEndDateAsync(newUser, DateTimeOffset.MaxValue);
                    else
                        await _userManager.SetLockoutEndDateAsync(newUser, null);
                    //create user Identity for player - END

                    _context.Add(player);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator." + err.ToString());
            }
            //catch (RetryLimitExceededException /* dex */)
            //{
            //    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            //}
            //catch (DataException dex)
            //{

            //    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            //}
            //catch (Exception e)
            //{

            //    ModelState.AddModelError("", "Unable to spit");
            //}

            //PopulateAssignedPositionData(player);         

            //ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            //ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
            //ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "Name");
            PopulateDropDownListTeam(player);
            PopulateDropDownListPosition(player);
            //PopulateDropDownListPosition(player);
            return View(player);
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
            PopulateDropDownListTeam(player);
            PopulateDropDownListPosition(player);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, /*[Bind("ID,FirstName,LastName,Email,DOB,TeamID?,PositionID?")]*/ Player player, string chkRemoveImage, IFormFile thePicture)
        {
            var playerToUpdate = await _context.Players
                .SingleOrDefaultAsync(d => d.ID == id);

            if (playerToUpdate == null)
            {
                return NotFound();
            }
            

            if (await TryUpdateModelAsync<Player>(playerToUpdate, "",
                d => d.FirstName, d => d.LastName, d => d.Email, d => d.TeamID, d => d.PositionID))
            {
                try
                {
                    if (chkRemoveImage != null)
                    {
                        playerToUpdate.imageContent = null;
                        playerToUpdate.imageMimeType = null;
                        playerToUpdate.imageFileName = null;
                    }
                    else
                    {
                        if (thePicture != null)
                        {
                            string mimeType = thePicture.ContentType;
                            long fileLength = thePicture.Length;
                            if (!(mimeType == "" || fileLength == 0))
                            {
                                if (mimeType.Contains("image"))
                                {
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        await thePicture.CopyToAsync(memoryStream);
                                        playerToUpdate.imageContent = memoryStream.ToArray();
                                    }
                                    playerToUpdate.imageMimeType = mimeType;
                                    playerToUpdate.imageFileName = thePicture.FileName;
                                }
                            }
                        }
                    }
                    var user = await _userManager.FindByEmailAsync(playerToUpdate.Email);

                    if (!player.IsEnabled)
                    {
                        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                        playerToUpdate.IsEnabled = false;
                    }
                    else
                    {
                        await _userManager.SetLockoutEndDateAsync(user, null);
                        playerToUpdate.IsEnabled = true;
                    }
                        

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(playerToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateDropDownListTeam(player);
            PopulateDropDownListPosition(player);
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
                Player player = new Player();
                player.FirstName = workSheet.Cells[row, 1].Text;
                player.LastName = workSheet.Cells[row, 2].Text;
                player.Email = workSheet.Cells[row, 3].Text;
                player.DOB = Convert.ToDateTime(workSheet.Cells[row, 4].Value);

                if (!String.IsNullOrWhiteSpace(workSheet.Cells[row, 5].Text))
                {
                    player.PositionID = _context.Positions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 5].Text).ID;
                }

                if (!String.IsNullOrWhiteSpace(workSheet.Cells[row, 6].Text))
                {
                    player.TeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 6].Text).ID;
                }

                // Row by row...
                //Player player = new Player
                //{
                //    FirstName = workSheet.Cells[row, 1].Text,
                //    LastName = workSheet.Cells[row, 2].Text,
                //    Email = workSheet.Cells[row, 3].Text,
                //    DOB = Convert.ToDateTime(workSheet.Cells[row, 4].Value),
                //    PositionID = _context.Positions.FirstOrDefault(d => d.Name == workSheet.Cells[row, 5].Text).ID,
                //    TeamID = _context.Teams.FirstOrDefault(d => d.Name == workSheet.Cells[row, 6].Text).ID
                //};

                //create user Identity for player - BEGIN
                IdentityUser newUser = new IdentityUser
                {
                    UserName = player.Email,
                    Email = player.Email
                };

                IdentityResult result = _userManager.CreateAsync(newUser, "password").Result;

                if (result.Succeeded)
                    _userManager.AddToRoleAsync(newUser, "User").Wait();
                //create user Identity for player - END


                _context.Players.Add(player);
            };
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        

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