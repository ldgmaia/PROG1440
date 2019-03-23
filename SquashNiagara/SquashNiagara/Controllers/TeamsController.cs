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
using Microsoft.AspNetCore.Identity;

namespace SquashNiagara.Controllers
{
    public class TeamsController : Controller
    {
        private readonly SquashNiagaraContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TeamsController(SquashNiagaraContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            var dQuery = from d in _context.Players
                         orderby d.FullName
                         where d.TeamID == id
                         select d;
            
            if (team == null)
            {
                return NotFound();
            }

            //var dQuery = _context.Players
             //   .FirstOrDefault(p => p.TeamID == id);

            ViewData["TeamPlayers"] = dQuery;

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            //ViewData["CaptainID"] = new SelectList(_context.Players, "ID", "Email");
            PopulateDropDownListCaptain();
            PopulateDropDownListVenue();
            PopulateDropDownListSeason();
            PopulateDropDownListDivision();
            //ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name, CaptainID, VenueID, Profile")] Team team,  
            IFormFile thePicture, int SeasonID, int DivisionID)
        {
            bool error = false;

            if (SeasonID == 0)
            {
                if (DivisionID == 0)
                {
                    ModelState.AddModelError("", "Please, select a Season and Division");
                    error = true;
                } else
                {
                    ModelState.AddModelError("", "Please, select a Season");
                    error = true;
                }
            } else if (SeasonID == 0)
            {
                ModelState.AddModelError("", "Please, select a Division");
                error = true;
            }

            if (error == true)
            {
                PopulateDropDownListCaptain();
                PopulateDropDownListVenue();
                PopulateDropDownListSeason();
                PopulateDropDownListDivision();
                return View();
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

                    int teamID = _context.Teams.FirstOrDefault(d => d.Name == team.Name).ID;
                    
                    var checkExistSeasonDivisionTeam = _context.SeasonDivisionTeams.FirstOrDefault(d => d.SeasonID == SeasonID && d.DivisionID == DivisionID && d.TeamID == teamID);
                    if (checkExistSeasonDivisionTeam == null)
                    {
                        SeasonDivisionTeam seasonDivisionTeam = new SeasonDivisionTeam
                        {
                            SeasonID = SeasonID,
                            DivisionID = DivisionID,
                            TeamID = teamID
                        };
                        _context.SeasonDivisionTeams.Add(seasonDivisionTeam);
                        _context.SaveChanges();
                    }

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
        public async Task<IActionResult> Edit(int id, /*[Bind("ID, Name, CaptainID, VenueID, Profile")]*/ Team team, string chkRemoveImage, IFormFile thePicture)
        {
            //Add Role to captain - BEGIN
            var userToUpdate = await _context.Players
                .SingleOrDefaultAsync(d => d.TeamID == id && d.ID == team.CaptainID);

            var user = await _userManager.FindByEmailAsync(userToUpdate.Email);

            var newRoleToCaptain = _userManager.AddToRoleAsync(user, "Captain");
            //Add Role to captain - END

            var teamToUpdate = await _context.Teams
                .SingleOrDefaultAsync(d => d.ID == id);

            if (teamToUpdate == null)
            {
                return NotFound();
            }
            //if (team.CaptainID == 0)
            //{
            //    team.CaptainID = null;
            //}
            //if (team.VenueID == 0)
            //{
            //    team.VenueID = null;
            //}

            //if (team == null)
            //{
            //    return NotFound();
            //}
            if (await TryUpdateModelAsync<Team>(teamToUpdate, "",
                d => d.Name, d => d.CaptainID, d => d.VenueID, d => d.Profile))
            {
                try
                {
                    if (chkRemoveImage != null)
                    {
                        teamToUpdate.imageContent = null;
                        teamToUpdate.imageMimeType = null;
                        teamToUpdate.imageFileName = null;
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
                                        teamToUpdate.imageContent = memoryStream.ToArray();
                                    }
                                    teamToUpdate.imageMimeType = mimeType;
                                    teamToUpdate.imageFileName = thePicture.FileName;
                                }
                            }
                        }
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
                    if (!TeamExists(teamToUpdate.ID))
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

        private void PopulateDropDownListSeason(Team team = null)
        {
            var dQuery = from d in _context.Seasons
                         orderby d.Name
                         select d;
            int seasonID = 0;
            if (team != null)
            {
                seasonID = _context.SeasonDivisionTeams.FirstOrDefault(d => d.TeamID == team.ID).SeasonID;
                ViewData["SeasonID"] = new SelectList(dQuery, "ID", "Name", seasonID);
            } else
            {
                ViewData["SeasonID"] = new SelectList(dQuery, "ID", "Name");
            }
           
        }

        private void PopulateDropDownListDivision(Team team = null)
        {
            var dQuery = from d in _context.Divisions
                         orderby d.Name
                         select d;

            int divisionID = 0;
            if (team != null)
            {
                divisionID = _context.SeasonDivisionTeams.FirstOrDefault(d => d.TeamID == team.ID).DivisionID;
                ViewData["DivisionID"] = new SelectList(dQuery, "ID", "Name", divisionID);
            } else
            {
                ViewData["DivisionID"] = new SelectList(dQuery, "ID", "Name");
            }
            
        }

    }
}
