using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SquashNiagara.Data;
using SquashNiagara.Models;

namespace SquashNiagara.Controllers
{
    public class TeamsController : Controller
    {
        private readonly SquashNiagaraContext _context;

        public TeamsController(SquashNiagaraContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Captain") || User.IsInRole("User"))
            {
                var email = User.Identity.Name;
                var player = _context.Players
                .Include(t => t.Team)
                .Include(t => t.Position)
                .FirstOrDefault(m => m.Email == User.Identity.Name);
                if (player.TeamID.HasValue)
                {
                    return RedirectToAction("Details", new { id = player.TeamID }); 
                }
                else
                {
                    return View("NoTeamAssigned");
                }
               

            }

            var squashNiagaraContext = _context.Teams.Include(t => t.Captain).Include(t => t.Venue);
            return View(await squashNiagaraContext.ToListAsync());
        }

        public IActionResult NoTeamAssigned()
        {
            return View();
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Captain)
                .Include(t => t.Venue)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            //ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            PopulateDropDownListCaptain();
            PopulateDropDownListVenue();
            //ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name, CaptainID, VenueID, Profile")] Team team,  
            IFormFile thePicture)
        {
            if (team.CaptainID == 0)
            {
                team.CaptainID = null;
            }
            if (team.VenueID == 0)
            {
                team.VenueID = null;
            }
            try {
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
                                    team.imageContent = memoryStream.ToArray();
                                }
                                team.imageMimeType = mimeType;
                                team.imageFileName = thePicture.FileName;
                            }
                        }
                    }

                    _context.Add(team);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator." + err.ToString());
            }
            //ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email", team.CaptainID);
            PopulateDropDownListCaptain(team);
            PopulateDropDownListVenue(team);
            //ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", team.VenueID);
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            PopulateDropDownListCaptain(team);
            PopulateDropDownListVenue(team);
           // ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", team.VenueID);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name, CaptainID, VenueID, Profile")] Team team, string chkRemoveImage, IFormFile thePicture)
        {
            if (team.CaptainID == 0)
            {
                team.CaptainID = null;
            }
            if (team.VenueID == 0)
            {
                team.VenueID = null;
            }

            if (team == null)
            {
                return NotFound();
            }
                       
            try
            {
                if (ModelState.IsValid)
                {
                    if (chkRemoveImage != null)
                    {
                        team.imageContent = null;
                        team.imageMimeType = null;
                        team.imageFileName = null;
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
                                        team.imageContent = memoryStream.ToArray();
                                    }
                                    team.imageMimeType = mimeType;
                                    team.imageFileName = thePicture.FileName;
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(team.ID))
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

            PopulateDropDownListCaptain(team);
            PopulateDropDownListVenue(team);
            //ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name", team.VenueID);
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Captain)
                .Include(t => t.Venue)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID == id);
        }

        private void PopulateDropDownListCaptain(Team team = null)
        {
            var dQuery = from d in _context.Players
                         orderby d.FirstName, d.LastName                       
                         select d;
            ViewData["CaptainID"] = new SelectList(dQuery, "ID", "FullName", team?.Captain);
        }

        private void PopulateDropDownListVenue(Team team = null)
        {
            var dQuery = from d in _context.Venues
                         orderby d.Name
                         select d;
            ViewData["VenueID"] = new SelectList(dQuery, "ID", "Name", team?.Venue);
        }

    }
}
